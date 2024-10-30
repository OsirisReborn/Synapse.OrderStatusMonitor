using Synapse.OrderStatus.Api.Workers;

namespace Synapse.OrderStatus.API.Extensions;

public static class WorkerServiceExtensions
{
    public static IServiceCollection AddWorkerServiceExtensions(this IServiceCollection services, IConfiguration config)
    {        
        services.AddHostedService<OrderMonitorService>();
        return services;
    }
}
