using Contract.Events;
using CoordinatorService.Application.Abstractions;
using MassTransit;

namespace CoordinatorService.Application.Consumers;

public class HungryEventConsumer(ICoordinatorManager coordinatorManager, IPublishEndpoint publishEndpoint) : IConsumer<HungryEvent>
{
    public async Task Consume(ConsumeContext<HungryEvent> context)
    {
        var philosopherId = context.Message.PhilosopherId;
        coordinatorManager.EnqueueHungry(philosopherId);
        if (coordinatorManager.TryGrantPermissionToEat(out var allowedPhilosopherId))
        {
            await publishEndpoint.Publish(new EatingPermissionGrantedEvent(allowedPhilosopherId));
        }
    }
}