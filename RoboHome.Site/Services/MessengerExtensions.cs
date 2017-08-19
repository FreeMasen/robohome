using Microsoft.Extensions.DependencyInjection;
namespace RoboHome.Services
{
    public static class MessengerExtensions
    {
        public static void AddMqClient(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IMqClient>(new Messenger(connectionString));
        }
    }
}