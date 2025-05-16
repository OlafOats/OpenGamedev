using OpenGamedev.Data;

namespace OpenGamedev.Services.Interfaces
{
    public interface IAcceptedSolutionProcessor
    {
        Task ProcessAcceptedSolutionsAsync(OpenGamedevContext dbContext, CancellationToken stoppingToken);
    }
}
