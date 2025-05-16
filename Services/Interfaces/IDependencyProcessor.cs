using OpenGamedev.Data;

namespace OpenGamedev.Services.Interfaces
{
    public interface IDependencyProcessor
    {
        Task ProcessDependenciesAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken);
    }
}
