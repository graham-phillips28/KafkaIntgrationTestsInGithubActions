using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;

namespace Tests
{
    public class KafkaTests
    {
        private readonly ILogger<KafkaTests> _logger;

        [Fact]
        public async Task Test1Async()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "127.0.0.1:9092",
            };

            var producer = new ProducerBuilder<Null, string>(config).Build();

            //var response = await producer.ProduceAsync("test", new Message<Null, string> { Value = "message" });
            
        }

        [Fact]
        public async Task Test2Async()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            //consumer.Subscribe("test");

            //var consumeResult = consumer.Consume(new CancellationToken());
            //consumer.Close();
        }
    }
}