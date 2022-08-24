using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;

namespace Tests
{
    public class KafkaTests
    {

        [Fact]
        public async Task ProduceToTopicThenConsume()
        {
            //Arrange
            var testMessage = "test message";
            var consumer = GetTestConsumer();
            var producer = GetTestProducer();

            //Act
            await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = testMessage });
            consumer.Subscribe("test-topic");
            var consumeResult = consumer.Consume(new CancellationToken());
            consumer.Close();

            //Assert
            Assert.Equal(testMessage, consumeResult.Message.Value);
        }

        public IProducer<Null, string> GetTestProducer()
        {
            var produceConfig = new ProducerConfig
            {
                BootstrapServers = "kafka1:19092",
            };

            return new ProducerBuilder<Null, string>(produceConfig).Build();
        }

        public IConsumer<Null, string> GetTestConsumer()
        {
            var consumeConfig = new ConsumerConfig
            {
                BootstrapServers = "kafka1:19092",
                GroupId = "test-group-id",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Null, string>(consumeConfig).Build();
        }
    }
}