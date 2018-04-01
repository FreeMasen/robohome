using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RoboHome.Data;
using RoboHome.Models;
using RoboHome.Services;

namespace RoboHome.Extensions
{
    
    public static class FlipSchedulerExtensions
    {
        public static IServiceCollection AddFlipScheduler(this IServiceCollection services, string connStr, string weatherServiceUri)
        {
            services.AddSingleton<IHostedService, FlipScheduler>(options => {
                var optionsBuilder = new DbContextOptionsBuilder<RoboContext>();
                optionsBuilder.UseNpgsql(connStr);
                var context = new RoboContext(optionsBuilder.Options);
                return new FlipScheduler(weatherServiceUri, context);
            });

            return services;
        }

        public static IServiceCollection AddFlipExecuter(this IServiceCollection services, string connStr, IConfigurationSection mqConfig)
        {
            services.AddSingleton<IHostedService, FlipExecuter>(options => {
                var mq = new Messenger(mqConfig);
                var optionsBuilder = new DbContextOptionsBuilder<RoboContext>();
                optionsBuilder.UseNpgsql(connStr);
                var context = new RoboContext(optionsBuilder.Options);
                return new FlipExecuter(context, mq);
            });
            return services;
        }

    }
}