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
        private IModel Channel;
        
        public Messenger(string connectionString) 
        {
            var factory = new ConnectionFactory();
            factory.Uri = connectionString;
            this.Connection = factory.CreateConnection();
        }

        public void SendMessage(int raspberryPi, object message) 
        {
            var json = JsonConvert.SerializeObject(message);
            var msg = Encoding.UTF8.GetBytes(json);
            var queueName = raspberryPi.ToString();
            using (var channel = this.Connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.BasicPublish("", queueName, true, props, msg);
            }
        }
    }
}