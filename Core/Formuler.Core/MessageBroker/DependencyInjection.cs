using Formuler.Core.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Formuler.Core.MessageBroker
{
    public static class DependencyInjection
    {
        public static void AddRabbitMQ(this IServiceCollection serviceCollection, IConfiguration configuration, params Type[] consumers)
        {

            var messageBrokerSettingsSection = configuration.GetSection(nameof(MessageBrokerSettings));
            serviceCollection.Configure<MessageBrokerSettings>(messageBrokerSettingsSection);

            serviceCollection.AddMassTransit(x =>
            {
                foreach (var consumer in consumers)
                {
                    x.AddConsumer(consumer);
                }

                x.UsingRabbitMq((context, cfg) =>
                {
                    var messageBrokerSettings = messageBrokerSettingsSection.Get<MessageBrokerSettings>();
                    cfg.Host(messageBrokerSettings.Host, messageBrokerSettings.VirtualHost, h =>
                    {
                        h.Username(messageBrokerSettings.Username);
                        h.Password(messageBrokerSettings.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            // OPTIONAL, but can be used to configure the bus options
            serviceCollection.AddOptions<MassTransitHostOptions>()
                .Configure(options =>
                {
                    // if specified, waits until the bus is started before
                    // returning from IHostedService.StartAsync
                    // default is false
                    options.WaitUntilStarted = true;

                    // if specified, limits the wait time when starting the bus
                    options.StartTimeout = TimeSpan.FromSeconds(10);

                    // if specified, limits the wait time when stopping the bus
                    options.StopTimeout = TimeSpan.FromSeconds(30);
                });
        }
    }
}
