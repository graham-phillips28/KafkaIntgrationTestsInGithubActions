using Confluent.Kafka;
using KafkaIntgrationTestsInGithubActions.Kafka;
using KafkaIntgrationTestsInGithubActions.Kafka.Settings;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntgrationTestsInGithubActions.MassTransit
{
    public static partial class Configuration
    {
        public static void ConfigureMassTransit(this IServiceCollection services,
            ConsumerSettings consumerSettings, ProducerSettings producerSettings, RabbitMessageBusSettings rabbitMessageBusSettings)
        {
            services.AddMassTransit(mt =>
            {

                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMessageBusSettings.Endpoint), h =>
                    {
                        h.Username(rabbitMessageBusSettings.Username);
                        h.Password(rabbitMessageBusSettings.Password);
                        
                    });
                });



                //Kafka 
                mt.AddRider(rider =>
                {
                    rider.AddConsumer<Consumer>();

                    rider.AddProducer<Message<Null, string>>(producerSettings.Topic);

                    rider.UsingKafka((context, k) =>
                    {
                        k.Host(producerSettings.Endpoint);

                        k.TopicEndpoint<Message<Null, string>>(consumerSettings.Topic, consumerSettings.GroupId, e =>
                        {
                            e.ConfigureConsumer<Consumer>(context);
                            e.AutoOffsetReset = AutoOffsetReset.Latest;
                        });
                    });

                });
            });
        }
    }
}
