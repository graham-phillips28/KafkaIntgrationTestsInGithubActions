using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace Tests
{
    public class KafkaTests
    {

        [Fact]
        public void ProduceToTopicThenConsumeAsync()
        {
            Console.WriteLine("Pausing to ensure app is running..");
            Thread.Sleep(10000);
            Console.WriteLine("Test starting..");
            //Arrange
            var testString = "test string";
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });
            var consumer = GetTestConsumer();
            var producer = GetTestProducer();

            //Consume old messages
            consumer.Subscribe("outbound-test-topic-1");
            var consumeOldMessages = consumer.Consume(3000);
            while(consumeOldMessages != null)
            {
                consumeOldMessages = consumer.Consume(3000);
            }
            //Act
            Console.WriteLine($"Attempt to produce message: {testMessage}");
            producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });
            Thread.Sleep(1000);
            var consumeResult = consumer.Consume(new CancellationToken());
            Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
            consumer.Close();
            var deserializedMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);

            //Assert
            Assert.Equal(testString + " changed by application", deserializedMessage.Value);
        }

        public IProducer<Null, string> GetTestProducer()
        {
            var produceConfig = new ProducerConfig
            {
                //BootstrapServers = "localhost:9092",
                BootstrapServers = "kafka1:19092",
            };

            return new ProducerBuilder<Null, string>(produceConfig).Build();
        }

        public IConsumer<Null, string> GetTestConsumer()
        {
            var consumeConfig = new ConsumerConfig
            {
                //BootstrapServers = "localhost:9092",
                BootstrapServers = "kafka1:19092",
                GroupId = "local-test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Null, string>(consumeConfig).Build();
        }
    }
}