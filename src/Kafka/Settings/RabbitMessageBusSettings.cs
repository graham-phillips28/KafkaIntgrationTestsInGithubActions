using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntgrationTestsInGithubActions.Kafka.Settings
{
    public class RabbitMessageBusSettings
    {
        public string Endpoint { get; }
        public string Username { get; }
        public string Password { get; }

        public RabbitMessageBusSettings(IConfiguration configuration)
        {
            var settings = configuration.GetSection(GetType().Name).GetChildren();

            Endpoint = settings.First(x => x.Key == "ENDPOINT").Value;
            Username = settings.First(x => x.Key == "USERNAME").Value;
            Password = settings.First(x => x.Key == "PASSWORD").Value;
        }
    }
}
