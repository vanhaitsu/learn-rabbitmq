using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Services;
using Constants = Shared.Common.Constants;

namespace Shared;

public static class Configuration
{
    public static IServiceCollection AddSharedServices(
        this IServiceCollection services,
        ConfigurationManager configuration,
        Action<IBusRegistrationConfigurator>? configureConsumers = null,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureBus = null)
    {
        services.AddSwaggerGen();
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter(); // Apply kebab-case naming convention to endpoints

            // Use RabbitMQ as transport
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration[Constants.RabbitMqHostPath] ?? throw new ArgumentNullException(), "/", h =>
                {
                    h.Username(configuration[Constants.RabbitMqUsernamePath] ?? throw new ArgumentNullException());
                    h.Password(configuration[Constants.RabbitMqPasswordPath] ?? throw new ArgumentNullException());
                });

                cfg.ConfigureEndpoints(context); // Automatically configure endpoints for consumers
                configureBus?.Invoke(cfg, context);
            });

            configureConsumers?.Invoke(x);
        });

        # region Dependency Injection

        services.AddSingleton<IRabbitMqService, RabbitMqService>();

        #endregion

        return services;
    }
}