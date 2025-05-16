using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;
using OpenGamedev.Services.Interfaces;

namespace OpenGamedev.Services.Processors
{

    public class ConflictChecker(ILogger<ConflictChecker> logger, IGitIntegrationService gitIntegrationService, INotificationService notificationService) : IConflictChecker
    {
        private readonly ILogger<ConflictChecker> _logger = logger;
        private readonly IGitIntegrationService _gitIntegrationService = gitIntegrationService; // Inject Git Service
        private readonly INotificationService _notificationService = notificationService; // Inject Notification Service

        public async Task PerformProactiveConflictCheckAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Performing proactive conflict check for Solutions...");

            // More efficient approach: Iterate through projects with relevant solutions
            var projectsWithActiveSolutions = await dbContext.Projects
                 .Where(p => p.FeatureRequests.Any(fr => fr.Solutions.Any(s => s.Status == SolutionStatus.Accepted || s.Status == SolutionStatus.SolutionVoting)))
                 .Include(p => p.FeatureRequests)
                     .ThenInclude(fr => fr.Solutions.Where(s => s.Status == SolutionStatus.Accepted || s.Status == SolutionStatus.SolutionVoting))
                         .ThenInclude(s => s.Author)
                 .ToListAsync(stoppingToken);

            if (projectsWithActiveSolutions.Count == 0)
            {
                _logger.LogInformation("No Projects found with Solutions in Accepted or SolutionVoting status.");
                return;
            }

            _logger.LogInformation("Found {ProjectCount} Projects with Solutions to check for proactive conflicts.", projectsWithActiveSolutions.Count);

            var solutionsToUpdate = new List<Solution>();

            foreach (var project in projectsWithActiveSolutions)
            {
                var repositoryPath = project.RepositoryLink;
                if (string.IsNullOrEmpty(repositoryPath))
                {
                    _logger.LogError("Repository link is missing for Project {ProjectId}. Skipping conflict check for this project.", project.Id);
                    continue;
                }

                // Optional: Get main branch head for logging (can be skipped if not needed frequently)
                // var mainBranchHead = await _gitIntegrationService.GetBranchHeadAsync(repositoryPath, "main", stoppingToken);
                // if (!string.IsNullOrEmpty(mainBranchHead))
                // {
                //      _logger.LogInformation("Current main branch head for project {ProjectId} ({RepositoryPath}): {MainBranchHead}", project.Id, repositoryPath, mainBranchHead);
                // }

                var solutionsToCheck = project.FeatureRequests
                    .SelectMany(fr => fr.Solutions)
                    .Where(s => s.Status == SolutionStatus.Accepted || s.Status == SolutionStatus.SolutionVoting)
                    .ToList();

                if (solutionsToCheck.Count == 0)
                {
                    _logger.LogInformation("No Solutions in Accepted or SolutionVoting status found for Project {ProjectId}.", project.Id);
                    continue;
                }

                _logger.LogInformation("Checking {SolutionCount} Solutions in Project {ProjectId} for proactive conflicts.", solutionsToCheck.Count, project.Id);


                foreach (var solution in solutionsToCheck)
                {
                    var sourceBranchName = await _gitIntegrationService.GetSolutionBranchNameAsync(solution, stoppingToken);
                    if (string.IsNullOrEmpty(sourceBranchName))
                    {
                        _logger.LogError("Could not determine source branch name for Solution {SolutionId} in Project {ProjectId}. Skipping conflict check.", solution.Id, project.Id);
                        continue;
                    }

                    bool hasConflict = await _gitIntegrationService.CheckForConflictsAsync(repositoryPath, sourceBranchName, "main", stoppingToken);

                    // Use the HasMergeConflicts property to track and notify
                    if (solution.HasMergeConflicts != hasConflict)
                    {
                        solution.HasMergeConflicts = hasConflict;
                        solutionsToUpdate.Add(solution);

                        if (hasConflict)
                        {
                            _logger.LogWarning("Proactive conflict check: Solution {SolutionId} for Task {TaskId} in {RepositoryPath} now has conflicts with main.", solution.Id, solution.FeatureRequest.Id, repositoryPath);
                            await _notificationService.NotifySolutionMergeConflict(solution.AuthorId, solution.Id, stoppingToken);
                        }
                        else
                        {
                            _logger.LogInformation("Proactive conflict check: Solution {SolutionId} for Task {TaskId} in {RepositoryPath} no longer has conflicts with main.", solution.Id, solution.FeatureRequest.Id, repositoryPath);
                            // Optional: Notify the author that conflicts are resolved (if they were previously notified)
                        }
                    }
                    else
                    {
                        // Status hasn't changed, just log current state
                        if (hasConflict)
                        {
                            _logger.LogInformation("Proactive conflict check: Solution {SolutionId} for Task {TaskId} in {RepositoryPath} still has conflicts with main.", solution.Id, solution.FeatureRequest.Id, repositoryPath);
                        }
                        else
                        {
                            _logger.LogInformation("Proactive conflict check: Solution {SolutionId} for Task {TaskId} in {RepositoryPath} still has no conflicts with main.", solution.Id, solution.FeatureRequest.Id, repositoryPath);
                        }
                    }
                }
            }

            if (solutionsToUpdate.Count != 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Saved changes: {UpdatedSolutionCount} solutions updated after proactive conflict check.", solutionsToUpdate.Count);
            }

            _logger.LogInformation("Proactive conflict check completed.");
        }
    }
}
