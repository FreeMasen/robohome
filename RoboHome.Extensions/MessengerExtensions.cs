using Microsoft.Extensions.DependencyInjection;
using RoboHome.Services;

namespace RoboHome.Extensions
{
    public static class MessengerExtensions
    {
        public static IServiceCollection AddMqClient(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IMqClient, Messenger>();
            return services;
        }
    }
}