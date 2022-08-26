using System;
using System.Collections.Generic;
namespace KafkaIntgrationTestsInGithubActions.Kafka.Settings
{
    public class ConsumerSettings
    {
        public string Endpoint { get; }
        public string Topic { get; }
        public string GroupId { get; }

        public ConsumerSettings(IConfiguration configuration)
        {
            var settings = configuration.GetSection(GetType().Name).GetChildren();

            Endpoint = settings.First(x => x.Key == "ENDPOINT").Value;
            Topic = settings.First(x => x.Key == "TOPIC").Value;
            GroupId = settings.First(x => x.Key == "GROUP_ID").Value;
        }
    }
}