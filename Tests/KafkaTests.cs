using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;

namespace Tests
{
    public class KafkaTests
    {

        [Fact]
        public async Task ProduceToTopicThenConsumeAsync()
        {
            //Arrange
            var testMessage = "{}";
            var consumer = GetTestConsumer();
            var producer = GetTestProducer();

            //Act
            Console.WriteLine($"Attempt to produce message: {testMessage}");
            await producer.ProduceAsync("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });
            Thread.Sleep(1000);
            consumer.Subscribe("outbound-test-topic-1");
            var consumeResult = consumer.Consume(new CancellationToken());
            Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
            consumer.Close();

            //Assert
            Assert.Equal(testMessage, consumeResult.Message.Value);
        }

        public IProducer<Null, string> GetTestProducer()
        {
            var produceConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };

            return new ProducerBuilder<Null, string>(produceConfig).Build();
        }

        public IConsumer<Null, string> GetTestConsumer()
        {
            var consumeConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group-id",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Null, string>(consumeConfig).Build();
        }
    }
}