using KafkaIntgrationTestsInGithubActions.MassTransit;
using MassTransit.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using KafkaIntgrationTestsInGithubActions.Kafka.Settings;
using KafkaIntgrationTestsInGithubActions.Kafka;
using KafkaIntgrationTestsInGithubActions.Kafka.Interfaces;
using KafkaIntgrationTestsInGithubActions.Application.Interfaces;
using KafkaIntgrationTestsInGithubActions.Application;
using Confluent.Kafka;
using System.Net;
using KafkaIntgrationTestsInGithubActions.TestHelpers;

namespace KafkaIntgrationTestsInGithubActions
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var consumerSettings = new ConsumerSettings(Configuration);
            services.AddSingleton(consumerSettings);
            services.AddSingleton<IConsumer<Message<Null, string>>, Consumer>();
            var producerSettings = new ProducerSettings(Configuration);
            services.AddSingleton(producerSettings);
            services.AddSingleton<IProducer, Producer>();

            //GithubActionTestHelpers.InitialiseTestTopicsAndResetTestConsumers(consumerSettings, producerSettings);
            
            services.ConfigureMassTransit(consumerSettings, producerSettings);
            services.AddSingleton<IStringHandler, StringHandler>();



            // http://localhost:5000/health, https://localhost:5001/health
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");
        }
    }
}