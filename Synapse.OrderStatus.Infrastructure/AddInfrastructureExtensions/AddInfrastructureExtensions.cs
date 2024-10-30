using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synapse.OrderStatus.Domain.Interfaces;
using Synapse.OrderStatus.Infrastructure.Services;

namespace Synapse.OrderStatus.Infrastructure.AddInfrastructureExtensions;
public static class AddInfrastructureExtensions
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<IApiAlertService, ApiAlertService>();
        services.AddHttpClient<IApiOrderService, ApiOrderService>();


        return services;

    }
}
