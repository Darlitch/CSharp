using System.Diagnostics;
using IServices;
using Model;

namespace GenericHostSimulation_lab3;

public class Simulation(SimulationOptions options, ITableManager tableManager)
{
    // private readonly IPhilosopherStrategy _philosopherStrategy;
    // private const long SimulationDuration = 100000;
    // private List<Fork> Forks { get; set; }
    // private List<PhilosopherHostedService> Philosophers { get; set; }
    private ITableManager _tableManager = tableManager;
    private long _simulationDuration = options.DurationSeconds * 1000;
    private int _displayUpdateInterval = options.DisplayUpdateInterval;
    private readonly Stopwatch _stopwatch = new();

    public void Run()
    {
        _stopwatch.Start();
        while (_stopwatch.ElapsedMilliseconds < _simulationDuration)
        {
            if (_stopwatch.ElapsedMilliseconds % _displayUpdateInterval == 0)
            {
                long currTime = _stopwatch.ElapsedMilliseconds;
            }
        }
    }

    
    // public void Run()
    // {
    //     _stopwatch.Start();
    //     Task[] tasks = new Task[Philosophers.Count];
    //     for (var i = 0; i < Philosophers.Count; ++i)
    //     {
    //         var index = i;
    //         tasks[index] = Task.Factory.StartNew(() => RunPhilosopher(Philosophers[index]), TaskCreationOptions.LongRunning);
    //     }
    //     while (_stopwatch.ElapsedMilliseconds < SimulationDuration)
    //     {
    //         if (_stopwatch.ElapsedMilliseconds % 200 == 0)
    //         {
    //             long currTime = _stopwatch.ElapsedMilliseconds;
    //             if (Philosophers.All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) && Forks.All(f => f.State == ForkState.InUse))
    //             {
    //                 Metrics.PrintFinalMetrics(new MetricDto(Philosophers, Forks, currTime));
    //                 Console.WriteLine($"Deadlock at {currTime} ms!");
    //                 return;
    //             }
    //             else
    //             {
    //                 Metrics.PrintMetrics(new MetricDto(Philosophers, Forks, currTime));
    //             }
    //         }
    //     }
    //     Metrics.PrintFinalMetrics(new MetricDto(Philosophers, Forks, SimulationDuration));
    // }

    
}