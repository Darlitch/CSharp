using Contract.Services;
using Contract.Services.Event;
using Contract.Services.Simulation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Model.DTO;
using Model.Enums;
using Services.PhilosopherMain;

namespace Services.Simulation;

public class Simulation(IOptions<SimulationOptions> options, IEnumerable<IHostedService> philosophers, IMetricsCollector metricsCollector,
    ITableManager tableManager, ISimulationTime simulationTime, IEventQueue eventQueue) : ISimulation
{
    private readonly long _simulationDuration = options.Value.DurationSeconds * 1000;
    private readonly int _displayUpdateInterval = options.Value.DisplayUpdateInterval;
    private readonly IEnumerable<PhilosopherHostedService> _philosophers = philosophers.OfType<PhilosopherHostedService>().ToList();
    
    public void Run()
    {
        while (simulationTime.CurrentTimeMs < _simulationDuration)
        {
            if (simulationTime.CurrentTimeMs % _displayUpdateInterval == 0)
            {
               var currTime = simulationTime.CurrentTimeMs;
                if (_philosophers.All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) && tableManager.AllInUse())
                {
                    metricsCollector.PrintFinalMetrics(currTime);
                    eventQueue.Enqueue(new UpdateSimulationRunDto(currTime));
                    Console.WriteLine($"Deadlock at {currTime} ms!");
                    return;
                }
                else
                {
                    metricsCollector.PrintMetrics(currTime);
                }
            }
        }
        metricsCollector.PrintFinalMetrics(simulationTime.CurrentTimeMs);
    }
}