using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntgrationTestsInGithubActions.Kafka.Settings
{
    public class ProducerSettings
    {
        public string Endpoint { get; set; }
        public string Topic { get; set; }

        public ProducerSettings(IConfiguration configuration)
        {
            var settings = configuration.GetSection(GetType().Name).GetChildren();

            Endpoint = settings.First(x => x.Key == "ENDPOINT").Value;
            Topic = settings.First(x => x.Key == "TOPIC").Value;
        }
    }
}
