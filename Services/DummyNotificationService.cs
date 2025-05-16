namespace OpenGamedev.Services
{
    public class DummyNotificationService(ILogger<DummyNotificationService> logger) : INotificationService
    {
        private readonly ILogger<DummyNotificationService> _logger = logger;

        public Task NotifyTaskReady(string userId, long taskId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Simulating notification to user {UserId}: Task {TaskId} is ready.", userId, taskId);
            return Task.CompletedTask;
        }

        public Task NotifyTaskReadyForImplementation(string userId, long taskId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Simulating notification to user {UserId}: Task {TaskId} is ready for implementation.", userId, taskId);
            return Task.CompletedTask;
        }

        public Task NotifySolutionMergeConflict(string userId, long solutionId, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Simulating notification to user {UserId}: Solution {SolutionId} has merge conflicts.", userId, solutionId);
            return Task.CompletedTask;
        }

        public Task NotifySolutionMerged(string userId, long solutionId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Simulating notification to user {UserId}: Solution {SolutionId} was merged.", userId, solutionId);
            return Task.CompletedTask;
        }

        public Task NotifyTaskSuperseded(string userId, long taskId, long supersededByTaskId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Simulating notification to user {UserId}: Task {TaskId} was superseded by task {SupersededByTaskId}.", userId, taskId, supersededByTaskId);
            return Task.CompletedTask;
        }

        public Task NotifySolutionRejected(string userId, long solutionId, string reason, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Simulating notification to user {UserId}: Solution {SolutionId} was rejected. Reason: {Reason}", userId, solutionId, reason);
            return Task.CompletedTask;
        }
    }


}
