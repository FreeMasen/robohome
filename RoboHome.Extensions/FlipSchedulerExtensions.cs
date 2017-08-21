using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RoboHome.Data;
using RoboHome.Models;
using RoboHome.Services;

namespace RoboHome.Extensions
{
    
    public static class FlipSchedulerExtensions
    {
        public static IServiceCollection AddFlipScheduler(this IServiceCollection services, string weatherServiceUri, string connectionString)
        {
            services.AddSingleton<FlipScheduler, FlipScheduler>(options => {
                var optionsBuilder = new DbContextOptionsBuilder<RoboContext>();
                optionsBuilder.UseNpgsql(connectionString);
                var context = new RoboContext(optionsBuilder.Options);
                return new FlipScheduler(weatherServiceUri, context);
            });
            return services;
        }
    }
}