using Contract.Services;
using Microsoft.Extensions.Hosting;
using Model.DTO;

namespace Services;

public class EventProcessorHostedService(IEventQueue queue, IRecordManager recordManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!recordManager.ReadyToStart)
        {
            await Task.Delay(10, stoppingToken);
        }
        await foreach (var evt in queue.ReadAllAsync().WithCancellation(stoppingToken))
        {
            switch (evt)
            {
                case PhilosopherEventDto dto:
                    await recordManager.RecordPhilosopherEvent(dto);
                    // Console.WriteLine($"{dto.Index} : {dto.CurrentTimeMs}");
                    break;
                case ForkEventDto dto:
                    await recordManager.RecordForkEvent(dto);
                    break;
                case SimulationRunDto dto:
                    await recordManager.UpdateSimulationRun(dto);
                    break;
                default:
                    Console.WriteLine("Unknown event");
                    break;
            }
        }
    }
}