using Confluent.Kafka;
using KafkaIntgrationTestsInGithubActions.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntgrationTestsInGithubActions.Kafka
{
    public class Consumer : IConsumer<Message<Null, string>>
    {
        private IStringHandler _handler;
        public Consumer(IStringHandler handler)
        {
            _handler = handler;
        }
        public async Task Consume(ConsumeContext<Message<Null, string>> context)
        {
            Console.WriteLine(context.Message.Value);
            await _handler.Handle(context.Message.Value);
        }
    }
}
