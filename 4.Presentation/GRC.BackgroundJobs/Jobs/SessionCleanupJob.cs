using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GRC.BackgroundJobs.Jobs;

public class SessionCleanupJob : BackgroundService
{
    private readonly ILogger<SessionCleanupJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SessionCleanupJob(ILogger<SessionCleanupJob> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var sessionService = scope.ServiceProvider.GetRequiredService<UserSessionService>();
                
                await sessionService.CleanupExpiredSessionsAsync();
                _logger.LogInformation("Session cleanup job completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during session cleanup job");
            }

            // 24 saatte bir çalıştır
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
