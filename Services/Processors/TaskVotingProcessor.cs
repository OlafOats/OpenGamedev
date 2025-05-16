using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;
using OpenGamedev.Services.Interfaces;

namespace OpenGamedev.Services.Processors
{

    public class TaskVotingProcessor(ILogger<TaskVotingProcessor> logger) : ITaskVotingProcessor
    {
        private readonly ILogger<TaskVotingProcessor> _logger = logger;

        public async Task ProcessTaskVotingWinnersAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking tasks in Task Voting for readiness based on votes and Work Area availability...");

            var votingTasks = await dbContext.FeatureRequests
                .Where(fr => fr.Status == FeatureRequestStatus.TaskVoting)
                .Include(fr => fr.Votes)
                .Include(fr => fr.FeatureRequestWorkAreas) 
                    .ThenInclude(frwa => frwa.WorkArea)
                .ToListAsync(stoppingToken);

            if (votingTasks.Count == 0)
            {
                _logger.LogInformation("No tasks found in TaskVoting status.");
                return;
            }

            var rankedTasks = votingTasks
                .Select(task => new
                {
                    Task = task,
                    NetVotes = task.Votes.Sum(v => v.VoteType == VoteType.Upvote ? 1 : (v.VoteType == VoteType.Downvote ? -1 : 0))
                })
                .OrderByDescending(t => t.NetVotes)
                .ThenBy(t => t.Task.CreationDate) 
                .ToList();

            _logger.LogInformation("Found {TaskCount} tasks in TaskVoting status. Ranked by net votes.", rankedTasks.Count);

            var tasksToMoveToReady = new List<FeatureRequest>();
            var tasksToClose = new List<FeatureRequest>(); 

            var occupiedWorkAreaIdsThisCycle = new HashSet<long>();

            var implementedTasksWithAreas = await dbContext.FeatureRequests
                .Where(fr => fr.Status == FeatureRequestStatus.Implemented)
                .Include(fr => fr.FeatureRequestWorkAreas)
                    .ThenInclude(frwa => frwa.WorkArea)
                .ToListAsync(stoppingToken);

            var occupiedWorkAreaIdsByImplemented = new HashSet<long>(
                implementedTasksWithAreas.SelectMany(fr => fr.FeatureRequestWorkAreas.Select(frwa => frwa.WorkAreaId))
            );

            _logger.LogInformation("Found {OccupiedAreaCount} Work Areas occupied by Implemented tasks.", occupiedWorkAreaIdsByImplemented.Count);


            foreach (var rankedTask in rankedTasks)
            {
                var task = rankedTask.Task;
                var netVotes = rankedTask.NetVotes;

                _logger.LogInformation("Evaluating Task '{TaskTitle}' (ID: {TaskId}) with {NetVotes} net votes.", task.Title, task.Id, netVotes);

                var requiredWorkAreaIds = task.FeatureRequestWorkAreas.Select(frwa => frwa.WorkAreaId).ToList();

                bool hasConflict = requiredWorkAreaIds.Any(areaId =>
                    occupiedWorkAreaIdsByImplemented.Contains(areaId) ||
                    occupiedWorkAreaIdsThisCycle.Contains(areaId)
                );

                if (!hasConflict)
                {
                    _logger.LogInformation("Task '{TaskTitle}' (ID: {TaskId}) - Work Areas are free. Moving to ReadyForImplementation.", task.Title, task.Id);
                    task.Status = FeatureRequestStatus.ReadyForImplementation;
                    tasksToMoveToReady.Add(task);

                    foreach (var areaId in requiredWorkAreaIds)
                    {
                        occupiedWorkAreaIdsThisCycle.Add(areaId);
                    }
                }
                else
                {
                    _logger.LogInformation("Task '{TaskTitle}' (ID: {TaskId}) cannot move to ReadyForImplementation. Work Areas are occupied.", task.Title, task.Id);
                }
            }

            if (tasksToMoveToReady.Count != 0 || tasksToClose.Count != 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Saved changes: {ReadyCount} tasks moved to ReadyForImplementation, {ClosedCount} tasks moved to Closed.", tasksToMoveToReady.Count, tasksToClose.Count);
            }

            _logger.LogInformation("Task Voting check completed.");
        }
    }
}
