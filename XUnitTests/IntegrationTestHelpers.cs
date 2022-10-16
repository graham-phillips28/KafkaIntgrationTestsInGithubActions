using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class IntegrationTestHelpers : IDisposable
    {
        public IntegrationTestHelpers(Message<Null, string> latestMessage)
        {
            UpdateLatestMessageInBackgorund(latestMessage);
            Console.WriteLine("IntegrationTestHelpers ctor: This should only be run once");
        }

        public void UpdateLatestMessageInBackgorund(Message<Null, string> latestMessage)
        {
            var consumer = GetTestConsumer();
            var consumeResult = consumer.Consume();

            Task.Run(() => {
                if (consumeResult != null)
                {
                    latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
                }
                else
                {
                    latestMessage = new Message<Null, string>();
                }
                while (true)
                {
                    consumeResult = consumer.Consume();
                    latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
                }
            });
        }
        public IConsumer<Null, string> GetTestConsumer()
        {
            var consumeConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                //BootstrapServers = "kafka1:19092",
                GroupId = "local-test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Null, string>(consumeConfig).Build();
        }

        public void Dispose()
        {
            Console.WriteLine("SomeFixture: Disposing SomeFixture");
        }
    }
}
