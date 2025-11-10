using Contract.Services;
using Microsoft.Extensions.Hosting;
using Model.DTO;

namespace Services;

public class EventProcessorHostedService(IEventQueue queue, IManager manager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!manager.ReadyToStart)
        {
            await Task.Delay(10, stoppingToken);
        }
        await foreach (var evt in queue.ReadAllAsync().WithCancellation(stoppingToken))
        {
            switch (evt)
            {
                case PhilosopherEventDto dto:
                    await manager.RecordPhilosopherEvent(dto);
                    // Console.WriteLine($"{dto.Index} : {dto.CurrentTimeMs}");
                    break;
                case ForkEventDto dto:
                    await manager.RecordForkEvent(dto);
                    break;
                default:
                    Console.WriteLine("Unknown event");
                    break;
            }
        }
    }
}