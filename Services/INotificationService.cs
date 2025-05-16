namespace OpenGamedev.Services
{
    public interface INotificationService
    {
        Task NotifyTaskReady(string userId, long taskId, CancellationToken cancellationToken);
        Task NotifyTaskReadyForImplementation(string userId, long taskId, CancellationToken cancellationToken);
        Task NotifySolutionMergeConflict(string userId, long solutionId, CancellationToken cancellationToken);
        Task NotifySolutionMerged(string userId, long solutionId, CancellationToken cancellationToken);
        Task NotifyTaskSuperseded(string userId, long taskId, long supersededByTaskId, CancellationToken cancellationToken);
        Task NotifySolutionRejected(string userId, long solutionId, string reason, CancellationToken cancellationToken);
    }

}
