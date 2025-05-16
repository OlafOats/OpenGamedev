using OpenGamedev.Data;

namespace OpenGamedev.Services.Interfaces
{
    public interface ITaskVotingProcessor
    {
        Task ProcessTaskVotingWinnersAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken);
    }
}
