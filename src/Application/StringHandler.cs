using Confluent.Kafka;
using KafkaIntgrationTestsInGithubActions.Application.Interfaces;
using KafkaIntgrationTestsInGithubActions.Kafka.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntgrationTestsInGithubActions.Application
{
    public class StringHandler : IStringHandler
    {
        private IProducer _producer;
        public StringHandler(IProducer producer)
        {
            _producer = producer;
        }
        public async Task Handle(string consumedString)
        {
            var message = new Message<Null, string>() { Value = consumedString + " changed by application" };
            await _producer.Produce(message);
        }
    }
}
