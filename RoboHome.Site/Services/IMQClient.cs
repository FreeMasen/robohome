
namespace RoboHome.Services
{
    public interface IMqClient
    {
        void SendMessage(int id, object message);
    }
}