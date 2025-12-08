using Contract.Events;
using CoordinatorService.Application.Abstractions;
using MassTransit;

namespace CoordinatorService.Application.Consumers;

public class EndEatingEventConsumer(ICoordinatorManager coordinatorManager, ISendEndpointProvider endpointProvider) : IConsumer<EndEatingEvent>
{
    public async Task Consume(ConsumeContext<EndEatingEvent> context)
    {
        var philosopherId = context.Message.PhilosopherId;
        coordinatorManager.EndEating(philosopherId);
        if (coordinatorManager.TryGrantPermissionToEat(out var allowedPhilosopherId))
        {
            var endpoint = await endpointProvider.GetSendEndpoint(new Uri($"queue:philosopher-{allowedPhilosopherId}"));
            await endpoint.Send(new EatingPermissionGrantedEvent(allowedPhilosopherId));
            // await publishEndpoint.Publish(new EatingPermissionGrantedEvent(allowedPhilosopherId));
        }
    }
}