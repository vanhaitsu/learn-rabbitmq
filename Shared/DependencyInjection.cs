using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common;
using Shared.Consumers;
using Shared.Interfaces;
using Shared.Services;

namespace Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddMassTransit(x =>
        {
            // Apply kebab-case naming convention to endpoints
            x.SetKebabCaseEndpointNameFormatter();

            // Use RabbitMQ as transport
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration[Constants.RabbitMqHostPath] ?? throw new ArgumentNullException(), "/", h =>
                {
                    h.Username(configuration[Constants.RabbitMqUsernamePath] ?? throw new ArgumentNullException());
                    h.Password(configuration[Constants.RabbitMqPasswordPath] ?? throw new ArgumentNullException());
                });

                // Automatically configure endpoints for consumers
                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<MessageConsumer>();
        });

        services.AddSingleton<IRabbitMqService, RabbitMqService>();

        return services;
    }
}