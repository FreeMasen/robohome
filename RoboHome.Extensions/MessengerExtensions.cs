using System;
using Microsoft.Extensions.DependencyInjection;
using RoboHome.Services;

namespace RoboHome.Extensions
{
    public static class MessengerExtensions
    {
        
        public static IServiceCollection AddMqClient(this IServiceCollection services, Action<MessengerOptions> optionsAction)
        {
            services.Configure<MessengerOptions>(optionsAction);
            services.AddScoped<IMqClient, Messenger>();
            return services;
        }
    }
}