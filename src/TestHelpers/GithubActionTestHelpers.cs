using Confluent.Kafka;
using KafkaIntgrationTestsInGithubActions.Kafka.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntgrationTestsInGithubActions.TestHelpers
{
    public static class GithubActionTestHelpers
    {
        public static void InitialiseTestTopicsAndResetTestConsumers(ConsumerSettings consumerSettings, ProducerSettings producerSettings)
        {
            //Initialise topics
            var config = new ProducerConfig
            {
                BootstrapServers = consumerSettings.Endpoint,
            };

            //Initialise inbound topic by producing
            var producer = new ProducerBuilder<Null, string>(config).Build();

            producer.Produce(consumerSettings.Topic, new Message<Null, string>
            { Value = "{}" });

            //Reset test consumer group offset
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = consumerSettings.Endpoint,
                GroupId = "local-test-group"
            };
            var consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    var offsets = partitions.Select(tp => new TopicPartitionOffset(tp, Offset.End));
                    return offsets;
                })
                .Build();

            //Initialise outbound topic
            producer.Produce(producerSettings.Topic, new Message<Null, string>
            { Value = "{}" });
            //Reset test consumer group offset
            var consumerConfig2 = new ConsumerConfig
            {
                BootstrapServers = producerSettings.Endpoint,
                GroupId = "local-test-group"
            };
            var consumer2 = new ConsumerBuilder<string, string>(consumerConfig)
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    var offsets = partitions.Select(tp => new TopicPartitionOffset(tp, Offset.End));
                    return offsets;
                })
                .Build();
        }
    }
}
