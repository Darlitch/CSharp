using Contract.Events;
using CoordinatorService.Application.Abstractions;
using MassTransit;

namespace CoordinatorService.Application.Consumers;

public class HungryEventConsumer(ICoordinatorManager coordinatorManager, ISendEndpointProvider endpointProvider) : IConsumer<HungryEvent>
{
    public async Task Consume(ConsumeContext<HungryEvent> context)
    {
        var philosopherId = context.Message.PhilosopherId;
        coordinatorManager.EnqueueHungry(philosopherId);
        if (coordinatorManager.TryGrantPermissionToEat(out var allowedPhilosopherId))
        {
            var endpoint = await endpointProvider.GetSendEndpoint(new Uri($"queue:philosopher-{allowedPhilosopherId}"));
            await endpoint.Send(new EatingPermissionGrantedEvent(allowedPhilosopherId));
        }
    }
}