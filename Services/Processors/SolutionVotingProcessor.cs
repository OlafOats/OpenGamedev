using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;
using OpenGamedev.Services.Interfaces;

namespace OpenGamedev.Services.Processors
{

    public class SolutionVotingProcessor(ILogger<SolutionVotingProcessor> logger, INotificationService notificationService) : ISolutionVotingProcessor
    {
        private readonly ILogger<SolutionVotingProcessor> _logger = logger;
        private readonly INotificationService _notificationService = notificationService; // Inject Notification Service

        public async Task ProcessSolutionVotingAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking tasks with active or expired Solution Voting...");

            var tasksWithSolutionsInVoting = await dbContext.FeatureRequests
                .Where(fr => fr.Status == FeatureRequestStatus.ReadyForImplementation || fr.Status == FeatureRequestStatus.SolutionVotingExpired)
                .Where(fr => fr.Solutions.Any(s => s.Status == SolutionStatus.SolutionVoting || s.Status == SolutionStatus.BuildSuccess))
                .Include(fr => fr.Solutions.Where(s => s.Status == SolutionStatus.SolutionVoting || s.Status == SolutionStatus.BuildSuccess))
                    .ThenInclude(s => s.Votes)
                .ToListAsync(stoppingToken);

            if (tasksWithSolutionsInVoting.Count == 0)
            {
                _logger.LogInformation("No tasks found with solutions in voting status.");
                return;
            }

            _logger.LogInformation("Found {TaskCount} tasks with solutions in voting status.", tasksWithSolutionsInVoting.Count);

            var tasksToUpdate = new List<FeatureRequest>();
            var solutionsToUpdate = new List<Solution>();

            foreach (var task in tasksWithSolutionsInVoting)
            {
                bool votingPeriodExpired = false;
                if (task.SolutionVotingStartTime.HasValue && task.AverageSolutionVotingDuration.HasValue)
                {
                    votingPeriodExpired = DateTime.UtcNow >= task.SolutionVotingStartTime.Value + task.AverageSolutionVotingDuration.Value;
                }

                if (!votingPeriodExpired && task.Status != FeatureRequestStatus.SolutionVotingExpired)
                {
                    _logger.LogInformation("Solution voting for Task '{TaskTitle}' (ID: {TaskId}) is ongoing. Period expires at {ExpirationTime}.",
                        task.Title, task.Id, task.SolutionVotingStartTime?.Add(task.AverageSolutionVotingDuration ?? TimeSpan.Zero));
                    continue;
                }

                _logger.LogInformation("Solution voting for Task '{TaskTitle}' (ID: {TaskId}) has expired.", task.Title, task.Id);

                Solution? winningSolution = null;
                int maxNetVotes = 0;

                var solutionsInVoting = task.Solutions
                    .Where(s => s.Status == SolutionStatus.SolutionVoting || s.Status == SolutionStatus.BuildSuccess)
                    .ToList();

                if (solutionsInVoting.Count == 0)
                {
                    _logger.LogInformation("Task '{TaskTitle}' (ID: {TaskId}) has no solutions in voting status. Moving to SolutionVotingExpired.", task.Title, task.Id);
                    task.Status = FeatureRequestStatus.SolutionVotingExpired;
                    tasksToUpdate.Add(task);
                }
                else
                {
                    foreach (var solution in solutionsInVoting)
                    {
                        int netVotes = solution.Votes.Sum(v => v.VoteType == VoteType.Upvote ? 1 : (v.VoteType == VoteType.Downvote ? -1 : 0));
                        _logger.LogInformation("Solution {SolutionId} for Task {TaskId} has {NetVotes} net votes.", solution.Id, task.Id, netVotes);

                        if (netVotes > maxNetVotes || (netVotes == maxNetVotes && (winningSolution == null || solution.CreationDate < winningSolution.CreationDate)))
                        {
                            maxNetVotes = netVotes;
                            winningSolution = solution;
                        }
                    }

                    if (winningSolution != null && maxNetVotes > 0)
                    {
                        _logger.LogInformation("Winning Solution found for Task '{TaskTitle}' (ID: {TaskId}): Solution {WinningSolutionId} with {NetVotes} net votes.",
                            task.Title, task.Id, winningSolution.Id, maxNetVotes);

                        winningSolution.Status = SolutionStatus.Accepted;
                        solutionsToUpdate.Add(winningSolution);

                        var otherSolutions = solutionsInVoting.Where(s => s.Id != winningSolution.Id).ToList();
                        foreach (var otherSolution in otherSolutions)
                        {
                            otherSolution.Status = SolutionStatus.Rejected;
                            solutionsToUpdate.Add(otherSolution);
                            await _notificationService.NotifySolutionRejected(otherSolution.AuthorId, otherSolution.Id, "Another solution won the voting.", stoppingToken);
                            _logger.LogInformation("Solution {OtherSolutionId} for Task {TaskId} rejected.", otherSolution.Id, task.Id);
                        }
                        // await _notificationService.NotifySolutionAccepted(winningSolution.AuthorId, winningSolution.Id, stoppingToken);
                    }
                    else
                    {
                        _logger.LogInformation("No winning Solution found for Task '{TaskTitle}' (ID: {TaskId}). Moving task to SolutionVotingExpired.", task.Title, task.Id);
                        task.Status = FeatureRequestStatus.SolutionVotingExpired;
                        tasksToUpdate.Add(task);

                        foreach (var solution in solutionsInVoting)
                        {
                            solution.Status = SolutionStatus.Rejected;
                            solutionsToUpdate.Add(solution);
                            await _notificationService.NotifySolutionRejected(solution.AuthorId, solution.Id, "Solution voting expired without a winner.", stoppingToken);
                            _logger.LogInformation("Solution {SolutionId} for Task {TaskId} rejected due to expired voting.", solution.Id, task.Id);
                        }
                    }
                }
            }

            if (tasksToUpdate.Count != 0 || solutionsToUpdate.Count != 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Saved changes: {UpdatedTaskCount} tasks and {UpdatedSolutionCount} solutions updated after solution voting check.", tasksToUpdate.Count, solutionsToUpdate.Count);
            }

            _logger.LogInformation("Solution Voting check completed.");
        }
    }
}
