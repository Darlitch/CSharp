using Contract.Events;
using MassTransit;
using PhilosopherService.Application.Abstractions;
using PhilosopherService.Application.Workers;

namespace PhilosopherService.Application.Consumers;

public class EatingPermissionGrantedEventConsumer(IEatingPermissionService eatingPermissionService) : IConsumer<EatingPermissionGrantedEvent>
{
    public Task Consume(ConsumeContext<EatingPermissionGrantedEvent> context)
    {
        eatingPermissionService.GrantPermission();
        return Task.CompletedTask;
    }
}