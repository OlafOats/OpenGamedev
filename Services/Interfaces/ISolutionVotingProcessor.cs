using OpenGamedev.Data;

namespace OpenGamedev.Services.Interfaces
{
    public interface ISolutionVotingProcessor
    {
        Task ProcessSolutionVotingAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken);
    }
}
