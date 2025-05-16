using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;
using OpenGamedev.Services.Interfaces;

namespace OpenGamedev.Services.Processors
{
    public class DependencyProcessor(ILogger<DependencyProcessor> logger /*, INotificationService notificationService */) : IDependencyProcessor
    {
        private readonly ILogger<DependencyProcessor> _logger = logger;

        public async Task ProcessDependenciesAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking tasks waiting for dependencies...");

            var waitingTasks = await dbContext.FeatureRequests
                .Where(fr => fr.Status == FeatureRequestStatus.WaitingDependency)
                .ToListAsync(stoppingToken);

            if (waitingTasks.Count == 0)
            {
                _logger.LogInformation("No tasks waiting for dependencies.");
                return;
            }

            _logger.LogInformation("Found {WaitingTaskCount} tasks waiting for dependencies.", waitingTasks.Count);

            var completedOrReadyTaskIds = await dbContext.FeatureRequests
                .Where(fr => fr.Status == FeatureRequestStatus.Implemented || fr.Status == FeatureRequestStatus.ReadyForImplementation)
                .Select(fr => fr.Id)
                .ToListAsync(stoppingToken);

            var tasksToUpdate = new List<FeatureRequest>();

            foreach (var task in waitingTasks)
            {
                var requiredDependencyIds = await dbContext.FeatureRequestDependencies
                    .Where(d => d.FeatureRequestId == task.Id)
                    .Select(d => d.DependsOnFeatureRequestId)
                    .ToListAsync(stoppingToken);

                bool allDependenciesMet = requiredDependencyIds.All(depId => completedOrReadyTaskIds.Contains(depId));

                if (allDependenciesMet)
                {
                    _logger.LogInformation("Task '{TaskTitle}' (ID: {TaskId}) - all dependencies met. Moving to ReadyForImplementation.", task.Title, task.Id);
                    task.Status = FeatureRequestStatus.ReadyForImplementation;
                    tasksToUpdate.Add(task);
                    // Optional: Add logic to notify the task author
                    // await _notificationService.NotifyTaskReady(task.AuthorId, task.Id, stoppingToken);
                }
                else
                {
                    var missingDependencyIds = requiredDependencyIds.Where(depId => !completedOrReadyTaskIds.Contains(depId)).ToList();
                    _logger.LogInformation("Task '{TaskTitle}' (ID: {TaskId}) is still waiting for dependencies: {MissingDependencyIds}.",
                        task.Title, task.Id, string.Join(", ", missingDependencyIds.Select(id => id.ToString())));
                }
            }

            if (tasksToUpdate.Count != 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Saved changes: {UpdatedTaskCount} tasks moved from waiting_dependency to ReadyForImplementation.", tasksToUpdate.Count);
            }

            _logger.LogInformation("Dependency check completed.");
        }
    }
}
