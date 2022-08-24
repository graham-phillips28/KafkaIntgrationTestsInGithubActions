using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;

namespace Tests
{
    public class KafkaTests
    {

        [Fact]
        public async Task Test1Async()
        {
            var testMessage = "test message";
            var produceConfig = new ProducerConfig
            {
                BootstrapServers = "kafka1:19092",
            };
            var consumeConfig = new ConsumerConfig
            {
                BootstrapServers = "kafka1:19092",
                GroupId = "test-group-id",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Null, string>(consumeConfig).Build();

            
            
            var producer = new ProducerBuilder<Null, string>(produceConfig).Build();
            Console.WriteLine("Console working");
            var response = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = testMessage });
            Console.WriteLine(response.ToString());

            consumer.Subscribe("test-topic");
            var consumeResult = consumer.Consume(new CancellationToken());
            Console.WriteLine(consumeResult.Message.Value);
            Assert.Equal(testMessage, consumeResult.Message.Value);
            consumer.Close();

        }
    }
}