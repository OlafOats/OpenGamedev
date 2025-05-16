using OpenGamedev.Data;
using OpenGamedev.Services.Interfaces;
using System.Diagnostics;

namespace OpenGamedev.Services
{
    public class TaskQueueBackgroundService(
        ILogger<TaskQueueBackgroundService> logger,
        IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly ILogger<TaskQueueBackgroundService> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        private readonly TimeSpan _conflictCheckInterval = TimeSpan.FromMinutes(30); // Пример интервала
        private DateTime _lastConflictCheck = DateTime.MinValue;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Queue Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Task Queue Background Service is performing a check cycle.");
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    // Создаем область для разрешения Scoped-сервисов
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var serviceProvider = scope.ServiceProvider;

                        // Разрешаем Scoped-сервисы
                        var dbContext = serviceProvider.GetRequiredService<OpenGamedevContext>();
                        var dependencyProcessor = serviceProvider.GetRequiredService<IDependencyProcessor>();
                        var taskVotingProcessor = serviceProvider.GetRequiredService<ITaskVotingProcessor>();
                        var solutionVotingProcessor = serviceProvider.GetRequiredService<ISolutionVotingProcessor>();
                        var acceptedSolutionProcessor = serviceProvider.GetRequiredService<IAcceptedSolutionProcessor>();
                        var conflictChecker = serviceProvider.GetRequiredService<IConflictChecker>();

                        // Выполняем задачи
                        await dependencyProcessor.ProcessDependenciesAsync(dbContext, stoppingToken);
                        await taskVotingProcessor.ProcessTaskVotingWinnersAsync(dbContext, stoppingToken);
                        await solutionVotingProcessor.ProcessSolutionVotingAsync(dbContext, stoppingToken);
                        await acceptedSolutionProcessor.ProcessAcceptedSolutionsAsync(dbContext, stoppingToken);

                        if (DateTime.UtcNow - _lastConflictCheck >= _conflictCheckInterval)
                        {
                            await conflictChecker.PerformProactiveConflictCheckAsync(dbContext, stoppingToken);
                            _lastConflictCheck = DateTime.UtcNow;
                        }
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Task Queue Background Service check cycle finished in {ElapsedMilliseconds}ms.", stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during the background task execution.");
                }

                // Задержка перед следующей итерацией
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("Task Queue Background Service is stopping.");
        }
    }
}