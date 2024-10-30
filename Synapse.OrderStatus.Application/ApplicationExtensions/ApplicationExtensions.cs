using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synapse.OrderStatus.Application.Services;
using Synapse.OrderStatus.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.OrderStatus.Application.ApplicationExtensions;
public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationExtensions(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IOrderService, OrderService>();        
        return services;
    }

}
