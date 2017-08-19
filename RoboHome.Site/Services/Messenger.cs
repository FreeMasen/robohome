using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RoboHome.Services
{
    public class Messenger: IMqClient
    {
        private IConnection Connection;
        public Messenger(string connectionString) 
        {
            var factory = new ConnectionFactory();
            var uri = new Uri(connectionString);
            factory.Uri = uri;
            this.Connection = factory.CreateConnection();
        }

        public void SendMessage(int remoteId, object message) 
        {
            var json = JsonConvert.SerializeObject(message);
            var msg = Encoding.UTF8.GetBytes(json);
            var queueName = remoteId.ToString();
            using (var channel = this.Connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.BasicPublish("", queueName, true, props, msg);
            }
        }
    }
}