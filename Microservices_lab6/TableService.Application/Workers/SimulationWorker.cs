using Application.Abstractions;
using Application.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Application.Workers;

public class SimulationWorker(IOptions<TableOptions> options, ITableManager tableManager, 
    IMetricReporter metricReporter, ISimulationTime simulationTime, IHostApplicationLifetime lifetime) : BackgroundService
{
    private readonly int _displayUpdateInterval = options.Value.DisplayUpdateInterval;
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!tableManager.IsReady())
        {
            await Task.Delay(10, ct);
        }
        simulationTime.Start();
        await Task.Delay(_displayUpdateInterval, ct);
        while (!tableManager.IsAllFinished())
        {
            var currTime = simulationTime.CurrentTimeMs;
            if (tableManager.IsDeadLock())
            {
                Console.WriteLine($"Deadlock at {currTime} ms!");
                break;
            }
            else
            {
                metricReporter.PrintMetrics(currTime);
            }
            await Task.Delay(_displayUpdateInterval, ct);
        }
        metricReporter.PrintFinalMetrics(simulationTime.CurrentTimeMs);
        lifetime.StopApplication();
    }
}