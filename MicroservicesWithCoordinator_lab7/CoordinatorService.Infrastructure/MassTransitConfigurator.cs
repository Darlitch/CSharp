using CoordinatorService.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoordinatorService.Infrastructure;

public static class MassTransitConfigurator
{
    public static IServiceCollection AddMassTransitConfigurator(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<HungryEventConsumer>();
            x.AddConsumer<EndEatingEventConsumer>();
            
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
                
                cfg.ReceiveEndpoint("coordinator-queue", e =>
                {
                    e.ConfigureConsumer<HungryEventConsumer>(context);
                    e.ConfigureConsumer<EndEatingEventConsumer>(context);
                });
            });
        });
        return services;
    }
}