using Application.Abstractions;
using Application.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Application.Workers;

public class SimulationWorker(IOptions<TableServiceOptions> options, ITableManager tableManager, 
    IMetricReporter metricReporter, ISimulationTime simulationTime, IHostApplicationLifetime lifetime) : BackgroundService
{
    private readonly int _displayUpdateInterval = options.Value.DisplayUpdateInterval;
    protected override async Task ExecuteAsync(CancellationToken st)
    {
        while (!tableManager.IsReady())
        {
            await Task.Delay(10, st);
        }
        simulationTime.Start();
        await Task.Delay(_displayUpdateInterval, st);
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
            await Task.Delay(_displayUpdateInterval, st);
        }
        metricReporter.PrintFinalMetrics(simulationTime.CurrentTimeMs);
        lifetime.StopApplication();
    }
}