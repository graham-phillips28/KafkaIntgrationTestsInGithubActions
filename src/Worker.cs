using Confluent.Kafka;
using MassTransit;
using MassTransit.Serialization;

namespace KafkaIntgrationTestsInGithubActions
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            using var scope = _serviceScopeFactory.CreateScope();
            var producer = scope.ServiceProvider.GetService<ITopicProducer<Message<Null, string>>>();
            if (producer == null)
                throw new NullReferenceException();
            

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //Console.WriteLine("App running");
                await Task.Delay(1000, stoppingToken);
            }
        }

        
    }
}