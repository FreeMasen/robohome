using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;

namespace RoboHome.Services
{
    public class Messenger: IMqClient
    {
        private IConnection Connection;
        public Messenger(IOptions<MessengerOptions> options) 
        {
            var factory = new ConnectionFactory() {
                Uri = options.Value.Address,
            };
            this.Connection = factory.CreateConnection();
        }

        public void SendMessage(int remoteId, object message) 
        {
            var json = JsonConvert.SerializeObject(message);
            var msg = Encoding.UTF8.GetBytes(json);

            var topic = remoteId.ToString();
            var exchangeName = "switches";
            using (var channel = this.Connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, "topic");

                var props = channel.CreateBasicProperties();
                channel.QueueDeclare("", false, false, false, null);
                channel.BasicPublish(exchangeName, topic, true, props, msg);
            }
        }
    }
}