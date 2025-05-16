using OpenGamedev.Data;

namespace OpenGamedev.Services.Interfaces
{
    public interface IConflictChecker
    {
        Task PerformProactiveConflictCheckAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken);
    }
}
