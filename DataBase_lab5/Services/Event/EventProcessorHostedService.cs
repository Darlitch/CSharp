using Contract.Services;
using Contract.Services.Event;
using Microsoft.Extensions.Hosting;
using Model.DTO;

namespace Services.Event;

public class EventProcessorHostedService(IEventQueue queue, IRecordManager recordManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // while (!recordManager.ReadyToStart)
        // {
        //     await Task.Delay(10, stoppingToken);
        // }
        var runId = await recordManager.RecordSimulationRun();
        try
        {
            await foreach (var evt in queue.ReadAllAsync().WithCancellation(stoppingToken))
            {
                switch (evt)
                {
                    case CreatePhilosopherEventDto dto:
                        await recordManager.RecordPhilosopherEvent(runId, dto);
                        Console.WriteLine($"{dto.Index} : {dto.CurrentTimeMs}");
                        break;
                    case CreateForkEventDto dto:
                        await recordManager.RecordForkEvent(runId, dto);
                        Console.WriteLine($"{dto.Index} : {dto.CurrentTimeMs}");
                        break;
                    case UpdateSimulationRunDto dto:
                        await recordManager.UpdateSimulationRun(runId, dto);
                        break;
                    default:
                        Console.WriteLine("Unknown event");
                        break;
                }
            }
        }
        finally
        {
            Console.WriteLine($"Simulation RunId: {runId}");
        }
    }
}