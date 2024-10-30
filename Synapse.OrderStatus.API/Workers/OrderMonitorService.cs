using Microsoft.Extensions.Diagnostics.HealthChecks;
using Synapse.OrderStatus.Domain.Interfaces;

namespace Synapse.OrderStatus.Api.Workers;
public class OrderMonitorService(IServiceProvider serviceProvider, 
    ILogger<OrderMonitorService> logger, IConfiguration config) 
    :BackgroundService, IHealthCheck
{ 
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);
    private DateTime _lastSucessRun = DateTime.UtcNow;
    private readonly int _healthCheckBreakPoint = config.GetSection("HealthCheckBreakPoint").Get<int>();
    

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        var elapsedSinceLastRun = DateTime.UtcNow - _lastSucessRun;
        if (elapsedSinceLastRun > TimeSpan.FromMinutes(_healthCheckBreakPoint))
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("OrderMonitorService is sickly and frail, we may need to perform cpr."));
        }
    
        return Task.FromResult(HealthCheckResult.Healthy("Norm;OrderMonitorService is a beast!."));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("OrderMonitorService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    var result = await orderService.ProcessOrdersAsync();

                    if (!result.Success)
                    {
                        logger.LogError("Order processing failed: {Message}", result.Message);
                    }
                    else
                    {
                        logger.LogInformation("Order processing completed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing orders.");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        logger.LogInformation("OrderMonitorService is stopping.");
    }

    private async Task<bool> IsExternalApiReachableAsync(string apiUrl)
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(apiUrl);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
