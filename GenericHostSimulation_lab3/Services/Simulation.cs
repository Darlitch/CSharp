using System.Diagnostics;
using IServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Model.Enums;

namespace Services;

public class Simulation(IOptions<SimulationOptions> options, IEnumerable<IHostedService> philosophers, IMetricsCollector metricsCollector, ITableManager tableManager) : ISimulation
{
    private readonly long _simulationDuration = options.Value.DurationSeconds * 1000;
    private readonly int _displayUpdateInterval = options.Value.DisplayUpdateInterval;
    private readonly Stopwatch _stopwatch = new();

    public void Run()
    {
        _stopwatch.Start();
        while (_stopwatch.ElapsedMilliseconds < _simulationDuration)
        {
            if (_stopwatch.ElapsedMilliseconds % _displayUpdateInterval == 0)
            {
               var currTime = _stopwatch.ElapsedMilliseconds;
                if (philosophers.OfType<PhilosopherHostedService>().All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) && tableManager.AllInUse())
                {
                    metricsCollector.PrintFinalMetrics(currTime);
                    Console.WriteLine($"Deadlock at {currTime} ms!");
                    return;
                }
                else
                {
                    metricsCollector.PrintMetrics(currTime);
                }
            }
        }
        metricsCollector.PrintFinalMetrics(_stopwatch.ElapsedMilliseconds);
    }
}