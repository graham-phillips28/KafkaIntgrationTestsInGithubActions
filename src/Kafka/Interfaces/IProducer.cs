
using Confluent.Kafka;

namespace KafkaIntgrationTestsInGithubActions.Kafka.Interfaces
{
    public interface IProducer
    {
        public Task Produce(Message<Null, string> messageToProduce);
    }
}
