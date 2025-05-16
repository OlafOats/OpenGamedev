using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;
using OpenGamedev.Services.Interfaces;

namespace OpenGamedev.Services.Processors
{
    public class AcceptedSolutionProcessor(ILogger<AcceptedSolutionProcessor> logger, IGitIntegrationService gitIntegrationService, INotificationService notificationService) : IAcceptedSolutionProcessor
    {
        private readonly ILogger<AcceptedSolutionProcessor> _logger = logger;
        private readonly IGitIntegrationService _gitIntegrationService = gitIntegrationService; // Inject Git Service
        private readonly INotificationService _notificationService = notificationService; // Inject Notification Service

        public async Task ProcessAcceptedSolutionsAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking for Accepted Solutions ready for merge...");

            var acceptedSolutions = await dbContext.Solutions
                .Where(s => s.Status == SolutionStatus.Accepted)
                .Include(s => s.FeatureRequest)
                    .ThenInclude(fr => fr.Project)
                .Include(s => s.Author)
                .ToListAsync(stoppingToken);

            if (acceptedSolutions.Count == 0)
            {
                _logger.LogInformation("No Accepted Solutions found ready for merge.");
                return;
            }

            _logger.LogInformation("Found {AcceptedSolutionCount} Accepted Solutions ready for merge.", acceptedSolutions.Count);

            var solutionsToUpdate = new List<Solution>();
            var tasksToUpdate = new List<FeatureRequest>();
            var implementedTasks = new List<FeatureRequest>();

            foreach (var solution in acceptedSolutions)
            {
                if (solution.FeatureRequest.Status == FeatureRequestStatus.Implemented ||
                    solution.FeatureRequest.Status == FeatureRequestStatus.Closed ||
                    solution.FeatureRequest.Status == FeatureRequestStatus.Superseded ||
                    solution.FeatureRequest.Status == FeatureRequestStatus.Cancelled)
                {
                    _logger.LogInformation("Skipping merge for Solution {SolutionId} as parent task {TaskId} is in status {TaskStatus}.",
                        solution.Id, solution.FeatureRequest.Id, solution.FeatureRequest.Status);
                    solution.Status = SolutionStatus.Rejected;
                    solutionsToUpdate.Add(solution);
                    await _notificationService.NotifySolutionRejected(solution.AuthorId, solution.Id, "Parent task status prevents merge.", stoppingToken);
                    continue;
                }

                var repositoryPath = solution.FeatureRequest.Project?.RepositoryLink;

                if (string.IsNullOrEmpty(repositoryPath))
                {
                    _logger.LogError("Repository link is missing for Project {ProjectId} associated with Task {TaskId} and Solution {SolutionId}. Skipping merge.",
                        solution.FeatureRequest.Project?.Id, solution.FeatureRequest.Id, solution.Id);
                    solution.Status = SolutionStatus.Rejected;
                    solutionsToUpdate.Add(solution);
                    await _notificationService.NotifySolutionRejected(solution.AuthorId, solution.Id, "Project repository path is missing.", stoppingToken);
                    continue;
                }

                _logger.LogInformation("Attempting to merge Solution {SolutionId} for Task {TaskId} into main in repository {RepositoryPath}.", solution.Id, solution.FeatureRequest.Id, repositoryPath);

                var sourceBranchName = await _gitIntegrationService.GetSolutionBranchNameAsync(solution, stoppingToken);

                if (string.IsNullOrEmpty(sourceBranchName))
                {
                    _logger.LogError("Could not determine source branch name for Solution {SolutionId}. Skipping merge.", solution.Id);
                    solution.Status = SolutionStatus.BuildFailed;
                    solutionsToUpdate.Add(solution);
                    await _notificationService.NotifySolutionRejected(solution.AuthorId, solution.Id, "Could not determine source branch.", stoppingToken);
                    continue;
                }

                bool mergeSuccessful = await _gitIntegrationService.TryMergeBranchAsync(repositoryPath, sourceBranchName, "main", stoppingToken);

                if (!mergeSuccessful)
                {
                    _logger.LogWarning("Merge failed for Solution {SolutionId}. Likely Git conflicts.", solution.Id);
                    solution.Status = SolutionStatus.Rejected;
                    solutionsToUpdate.Add(solution);
                    await _notificationService.NotifySolutionMergeConflict(solution.AuthorId, solution.Id, stoppingToken);
                }
                else
                {
                    _logger.LogInformation("Merge successful for Solution {SolutionId}. Updating statuses.", solution.Id);

                    // The solution is already Accepted, keep it that way or change to Merged if you add that status
                    // solution.Status = SolutionStatus.Accepted; // Or SolutionStatus.Merged
                    // solutionsToUpdate.Add(solution); // Already in Accepted, might not need to add again unless status changes
                    // await _notificationService.NotifySolutionMerged(solution.AuthorId, solution.Id, stoppingToken);

                    var parentTask = solution.FeatureRequest;
                    if (parentTask.Status != FeatureRequestStatus.Implemented)
                    {
                        parentTask.Status = FeatureRequestStatus.Implemented;
                        tasksToUpdate.Add(parentTask);
                        implementedTasks.Add(parentTask);
                        _logger.LogInformation("Parent Task {TaskId} status updated to Implemented.", parentTask.Id);

                        var otherSolutions = await dbContext.Solutions
                            .Where(s => s.FeatureRequestId == parentTask.Id && s.Id != solution.Id && s.Status != SolutionStatus.Rejected)
                            .ToListAsync(stoppingToken);
                        foreach (var otherSolution in otherSolutions)
                        {
                            otherSolution.Status = SolutionStatus.Rejected;
                            solutionsToUpdate.Add(otherSolution);
                            await _notificationService.NotifySolutionRejected(otherSolution.AuthorId, otherSolution.Id, "Another solution was accepted and merged.", stoppingToken);
                            _logger.LogInformation("Solution {OtherSolutionId} for Task {TaskId} rejected as another solution was merged.", otherSolution.Id, parentTask.Id);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Parent Task {TaskId} was already in Implemented status.", parentTask.Id);
                        var otherSolutions = await dbContext.Solutions
                            .Where(s => s.FeatureRequestId == parentTask.Id && s.Id != solution.Id && s.Status != SolutionStatus.Rejected)
                            .ToListAsync(stoppingToken);
                        foreach (var otherSolution in otherSolutions)
                        {
                            otherSolution.Status = SolutionStatus.Rejected;
                            solutionsToUpdate.Add(otherSolution);
                            await _notificationService.NotifySolutionRejected(otherSolution.AuthorId, otherSolution.Id, "Another solution was accepted and merged.", stoppingToken);
                            _logger.LogInformation("Solution {OtherSolutionId} for Task {TaskId} rejected as another solution was merged.", otherSolution.Id, parentTask.Id);
                        }
                    }
                }
            }

            if (solutionsToUpdate.Count != 0 || tasksToUpdate.Count != 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Saved changes: {UpdatedSolutionCount} solutions and {UpdatedTaskCount} tasks updated after merge attempts.", solutionsToUpdate.Count, tasksToUpdate.Count);
            }

            // Trigger checks for superseded tasks based on newly Implemented tasks
            foreach (var implementedTask in implementedTasks)
            {
                await CheckForSupersededTasksAsync(dbContext, implementedTask, stoppingToken);
            }

            _logger.LogInformation("Accepted Solution merge check completed.");
        }

        // Helper method called after a FeatureRequest transitions to Implemented status
        // Checks for lower-priority FeatureRequests that become obsolete
        // because a higher-priority FeatureRequest with the same Work Areas was implemented.
        private async Task CheckForSupersededTasksAsync(OpenGamedevContext dbContext, FeatureRequest implementedTask, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking for superseded tasks after implementing task ID: {ImplementedTaskId}", implementedTask.Id);

            var implementedTaskRequiredAreaIds = await dbContext.FeatureRequestWorkAreas
                .Where(frwa => frwa.FeatureRequestId == implementedTask.Id)
                .Select(frwa => frwa.WorkAreaId)
                .ToListAsync(stoppingToken);

            if (implementedTaskRequiredAreaIds.Count == 0)
            {
                _logger.LogInformation("Implemented task {ImplementedTaskId} has no associated Work Areas. No tasks can be superseded by it based on area match.", implementedTask.Id);
                return;
            }

            var potentialSupersededTasks = await dbContext.FeatureRequests
                .Where(fr =>
                       fr.Status != FeatureRequestStatus.Implemented &&
                       fr.Status != FeatureRequestStatus.Closed &&
                       fr.Status != FeatureRequestStatus.Cancelled &&
                       fr.Status != FeatureRequestStatus.Superseded)
                .Where(fr => fr.ProjectId == implementedTask.ProjectId)
                .Include(fr => fr.FeatureRequestWorkAreas)
                .ToListAsync(stoppingToken);

            var tasksToSupersede = new List<FeatureRequest>();
            var solutionsToUpdate = new List<Solution>(); // Need a list for solutions to update here

            foreach (var potentialTask in potentialSupersededTasks)
            {
                // Requires FeatureRequest.Priority property
                if (potentialTask.Priority < implementedTask.Priority)
                {
                    var potentialTaskRequiredAreaIds = potentialTask.FeatureRequestWorkAreas.Select(frwa => frwa.WorkAreaId).ToList();

                    bool areasExactlyMatch = implementedTaskRequiredAreaIds.Count == potentialTaskRequiredAreaIds.Count &&
                                             implementedTaskRequiredAreaIds.All(potentialTaskRequiredAreaIds.Contains);

                    if (areasExactlyMatch)
                    {
                        _logger.LogInformation("Task '{PotentialTaskTitle}' (ID: {PotentialTaskId}) superseded by task '{ImplementedTaskTitle}' (ID: {ImplementedTaskId}). Lower priority and matching areas.",
                            potentialTask.Title, potentialTask.Id, implementedTask.Title, implementedTask.Id);

                        potentialTask.Status = FeatureRequestStatus.Superseded;
                        potentialTask.SupersededByFeatureRequestId = implementedTask.Id;
                        tasksToSupersede.Add(potentialTask);

                        var solutionsToReject = await dbContext.Solutions
                            .Where(s => s.FeatureRequestId == potentialTask.Id && s.Status != SolutionStatus.Rejected)
                            .ToListAsync(stoppingToken);

                        foreach (var solution in solutionsToReject)
                        {
                            solution.Status = SolutionStatus.Rejected;
                            _logger.LogInformation("Solution {SolutionId} for superseded Task {TaskId} rejected.", solution.Id, potentialTask.Id);
                            // await _notificationService.NotifySolutionRejected(solution.AuthorId, solution.Id, "Parent task was superseded.", stoppingToken);
                            solutionsToUpdate.Add(solution); // Add solution to the list to be saved
                        }
                    }
                }
            }

            // Save changes for tasksToSupersede AND the rejected solutions
            if (tasksToSupersede.Count != 0 || solutionsToUpdate.Count != 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Saved changes: {SupersededTaskCount} tasks marked as superseded and {RejectedSolutionCount} their solutions rejected.", tasksToSupersede.Count, solutionsToUpdate.Count);
            }

            _logger.LogInformation("Superseded tasks check completed.");
        }
    }
}
