using System;
using Microsoft.Extensions.Configuration;

namespace RoboHome.Services
{
    public class MessengerOptions
    {
        public string Host = "localhost";
        public int Port = 5672;
        public string UserName = "guest";
        public string Password = "guest";
        public Uri Address { 
            get
            {
                var builder = new UriBuilder();
                builder.UserName = this.UserName;
                builder.Password = this.Password;
                builder.Scheme = "amqp";
                builder.Host = this.Host;
                builder.Port = this.Port;
                return builder.Uri;
            }
        }
        public MessengerOptions()
        {
        }

        public void UseConfig(IConfigurationSection config)
        {
            if (config["host"] != null) 
            {
                this.Host = config["host"];
            }
            if (config["username"] != null) 
            {
                this.UserName = config["username"];
            }
            if (config["password"] != null) 
            {
                this.Password = config["password"];
            }
            if (config["port"] != null) 
            {
                int port;
                if (Int32.TryParse(config["port"], out port))
                {
                    this.Port = port;
                }
            }
        }
    }
}