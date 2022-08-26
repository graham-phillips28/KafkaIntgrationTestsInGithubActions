using Confluent.Kafka;
using KafkaIntgrationTestsInGithubActions.Kafka.Interfaces;
using MassTransit;
using MassTransit.Serialization;

namespace KafkaIntgrationTestsInGithubActions.Kafka
{
    public class Producer : IProducer
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITopicProducer<Message<Null, string>> _producer;

        public Producer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _producer = scope.ServiceProvider.GetService<ITopicProducer<Message<Null, string>>>() 
                ?? throw new NullReferenceException();
        }

        public async Task Produce(Message<Null,string> messageToProduce)
        {
            await _producer.Produce(messageToProduce);
        }
    }
}
