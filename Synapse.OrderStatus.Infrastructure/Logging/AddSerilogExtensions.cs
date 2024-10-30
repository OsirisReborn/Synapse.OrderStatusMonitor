using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Synapse.OrderStatus.Infrastructure.Logging;
public static class SerilogExtensions
{
    public static IServiceCollection AddSerilogExtensions(this IServiceCollection services, IConfiguration config)
    {


        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        return services;
    }
}
