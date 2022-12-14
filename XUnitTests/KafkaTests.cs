using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Threading;

namespace Tests
{
    public class KafkaTests
    {
        private Message<Null, string> _latestMessage;
        private IProducer<Null, string> _producer;
        private IConsumer<Null, string> _consumer;

        public KafkaTests()
        {
            _latestMessage = new Message<Null, string>();
            Thread.Sleep(10000);
            _producer = GetTestProducer();
            _consumer = GetTestConsumer();
            _consumer.Subscribe("outbound-test-topic-1");
            _producer = GetTestProducer();
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
            var consumeResult = _consumer.Consume();

            Task.Run(() =>
            {
                if (consumeResult != null)
                {
                    _latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
                }
                else
                {
                    _latestMessage = new Message<Null, string>();
                }
                while (true)
                {
                    consumeResult = _consumer.Consume();
                    _latestMessage = JsonConvert.DeserializeObject<Message<Null, string>>(consumeResult.Message.Value);
                }
            });
        }

        [Test]
        public void ProduceToTopicThenConsumeNew()
        {
            //Arrange
            var testString = "test string frwe";
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });
            Thread.Sleep(2000);

            //Assert
            Assert.Equals(testString + " changed by application", _latestMessage.Value);
        }

        [Test]
        public void ProduceToTopicThenConsumeNewTwo()
        {
            //Arrange
            var testString = "test string frwece";
            var testMessage = JsonConvert.SerializeObject(new Message<Null, string>() { Value = testString });

            //Act
            _producer.Produce("inbound-test-topic-1", new Message<Null, string>() { Value = testMessage });
            Thread.Sleep(2000);

            //Assert
            Assert.Equals(testString + " changed by application", _latestMessage.Value);
        }

        public IProducer<Null, string> GetTestProducer()
        {
            var produceConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                //BootstrapServers = "kafka1:19092",
            };
            return new ProducerBuilder<Null, string>(produceConfig).Build();
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
        }
    }
}