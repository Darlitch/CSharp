using Contract.Events;
using CoordinatorService.Application.Abstractions;
using MassTransit;

namespace CoordinatorService.Application.Consumers;

public class EndEatingEventConsumer(ICoordinatorManager coordinatorManager, IPublishEndpoint publishEndpoint) : IConsumer<EndEatingEvent>
{
    public async Task Consume(ConsumeContext<EndEatingEvent> context)
    {
        var philosopherId = context.Message.PhilosopherId;
        coordinatorManager.EndEating(philosopherId);
        if (coordinatorManager.TryGrantPermissionToEat(out var allowedPhilosopherId))
        {
            await publishEndpoint.Publish(new EatingPermissionGrantedEvent(allowedPhilosopherId));
        }
    }
}