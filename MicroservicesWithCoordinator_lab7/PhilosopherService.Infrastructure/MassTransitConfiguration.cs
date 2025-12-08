using Contract.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhilosopherService.Application.Consumers;

namespace PhilosopherService.Infrastructure;

public static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransitConfigurator(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<EatingPermissionGrantedEventConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration.GetValue<string>("RabbitMQ:Host") ?? "rabbitmq";
                var username = configuration.GetValue<string>("RabbitMQ:Username") ?? "guest";
                var password = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";
                
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                
                cfg.ReceiveEndpoint($"philosopher-{Environment.GetEnvironmentVariable("PhilosopherOptions__Id")}", e =>
                {
                    e.ConfigureConsumer<EatingPermissionGrantedEventConsumer>(context);
                });
            });
        });
        return services;
    }
}