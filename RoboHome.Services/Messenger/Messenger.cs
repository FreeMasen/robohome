using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using RoboHome.Models;

namespace RoboHome.Services
{
    public class Messenger: IMqClient
    {
        private IConnection Connection;
        public Messenger(IOptions<MessengerOptions> options)
        {
            this.Initialize(options.Value.Address);
        }

        public Messenger(IConfigurationSection config)
        {
            var ub = new UriBuilder();
            ub.Host = config.GetValue("host", "127.0.0.1");
            ub.Port = config.GetValue("port", 5672);
            ub.UserName = config.GetValue("username", "admin");
            ub.Password = config.GetValue("password", "password");
            ub.Scheme = "amqp";
            this.Initialize(ub.Uri);
        }

        private void Initialize(Uri address)
        {
            var factory = new ConnectionFactory() {
                Uri = address,
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
                channel.ExchangeDeclare(exchangeName, "topic", false);
                
                var props = channel.CreateBasicProperties();
                channel.QueueDeclare(exchangeName, false, false, false, null);
                channel.QueueBind(exchangeName, exchangeName, topic, null);
                channel.BasicPublish(exchangeName, topic, true, props, msg);
            }
        }

        public void SendMessage(int remoteId, Message message)
        {
            this.SendMessage(remoteId, message.ToSend());
        }
    }
    public struct Message
    {
        public int Switch { get; set; }
        public SwitchState State { get; set; }

        public object ToSend()
        {
            return new {
                switch_id = this.Switch,
                direction = this.State
            };
        }
    }

}