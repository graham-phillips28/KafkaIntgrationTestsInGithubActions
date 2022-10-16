using Confluent.Kafka;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Threading;

namespace NUnitTests
{
    public class KafkaTests
    {
        private Message<Null, string> _latestMessage;
        private IProducer<Null, string> _producer;
        private IConsumer<Null, string> _consumer;

        public KafkaTests()
        {
            _latestMessage = new Message<Null, string>();
            Thread.Sleep(5000);
            _producer = GetTestProducer();
            _consumer = GetTestConsumer();
            //var consumeResult = _consumer.Consume();

            //Task.Run(() =>
            //{
            //    if (consumeResult != null)
            //    {
            //        _latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
            //    }
            //    else
            //    {
            //        _latestMessage = new Message<Null, string>();
            //    }
            //    while (true)
            //    {
            //        consumeResult = _consumer.Consume();
            //        _latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
            //    }
            //});
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            //ConsumeResult<Null, string> consumeResult = new();
            _consumer.Subscribe("outbound-test-topic-1");
            Task.Run(() =>
            {
                //if (consumeResult != null)
                //{
                //    _latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
                //}
                //else
                //{
                //    _latestMessage = new Message<Null, string>();
                //}
                var consumeResult = _consumer.Consume();
                while (true)
                {
                    _latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
                    consumeResult = _consumer.Consume();
                }
            });
            Thread.Sleep(6000);
        }

        [Test]
        public void ProduceToTopicThenConsumeOne()
        {
            //Arrange
            var testString = "test string 1";
            Console.WriteLine("Producing to inbound topic: " + testString);
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });
            
            Thread.Sleep(2000);
            Console.WriteLine("Consumed from outbound topic: " + _latestMessage.Value);
            //Assert
            Assert.That(_latestMessage.Value, Is.EqualTo(testString + " changed by application"));
        }

        [Test]
        public void ProduceToTopicThenConsumeTwo()
        {
            //Arrange
            var testString = "test string 2";
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });
            Console.WriteLine("Producing to inbound topic: " + testString);

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });
            
            Thread.Sleep(2000);
            Console.WriteLine("Consumed from outbound topic: " + _latestMessage.Value);

            //Assert
            Assert.That(_latestMessage.Value, Is.EqualTo(testString + " changed by application"));
        }

        [Test]
        public void ProduceToTopicThenConsumeThree()
        {
            //Arrange
            var testString = "test string 3";
            Console.WriteLine("Producing to inbound topic: " + testString);
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });

            Thread.Sleep(2000);
            Console.WriteLine("Consumed from outbound topic: " + _latestMessage.Value);
            //Assert
            Assert.That(_latestMessage.Value, Is.EqualTo(testString + " changed by application"));
        }

        [Test]
        public void ProduceToTopicThenConsumeFour()
        {
            //Arrange
            var testString = "test string 4";
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });
            Console.WriteLine("Producing to inbound topic: " + testString);

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });

            Thread.Sleep(2000);
            Console.WriteLine("Consumed from outbound topic: " + _latestMessage.Value);

            //Assert
            Assert.That(_latestMessage.Value, Is.EqualTo(testString + " changed by application"));
        }

        [Test]
        public void ProduceToTopicThenConsumeFive()
        {
            //Arrange
            var testString = "test string 5";
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });
            Console.WriteLine("Producing to inbound topic: " + testString);

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });

            Thread.Sleep(2000);
            Console.WriteLine("Consumed from outbound topic: " + _latestMessage.Value);

            //Assert
            Assert.That(_latestMessage.Value, Is.EqualTo(testString + " changed by application"));
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

        public void Dispose()
        {
        }
    }
}