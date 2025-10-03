using System.Diagnostics;
using Model;
using Model.Enums;
using MultithreadedSimulation_lab2.DTO;
using StrategyInterface;

namespace MultithreadedSimulation_lab2;

public class Simulation
{
    private readonly IStrategy _strategy;
    private const long SimulationDuration = 100000;
    private List<Fork> Forks { get; set; }
    private List<Philosopher> Philosophers { get; set; }
    private readonly Stopwatch _stopwatch; 

    public Simulation(IStrategy strategy, List<Philosopher> philosophers)
    {
        Philosophers = philosophers;
        Forks = Philosophers.Select(p => p.LeftFork).ToList();
        _strategy = strategy;
        _stopwatch = new Stopwatch();
    }

    public void Run()
    {
        _stopwatch.Start();
        Task[] tasks = new Task[Philosophers.Count];
        for (var i = 0; i < Philosophers.Count; ++i)
        {
            var index = i;
            tasks[index] = Task.Factory.StartNew(() => RunPhilosopher(Philosophers[index]), TaskCreationOptions.LongRunning);
        }
        while (_stopwatch.ElapsedMilliseconds < SimulationDuration)
        {
            if (_stopwatch.ElapsedMilliseconds % 200 == 0)
            {
                long currTime = _stopwatch.ElapsedMilliseconds;
                if (Philosophers.All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) && Forks.All(f => f.State == ForkState.InUse))
                {
                    Metrics.PrintFinalMetrics(new MetricDto(Philosophers, Forks, currTime));
                    Console.WriteLine($"Deadlock at {currTime} ms!");
                    return;
                }
                else
                {
                    Metrics.PrintMetrics(new MetricDto(Philosophers, Forks, currTime));
                }
            }
        }
        Metrics.PrintFinalMetrics(new MetricDto(Philosophers, Forks, SimulationDuration));
    }

    private void RunPhilosopher(Philosopher philosopher)
    {
        while (_stopwatch.ElapsedMilliseconds < SimulationDuration)
        {
            philosopher.Update();
            if (philosopher is { IsHungry: true, Action: PhilosopherAction.None })
            {
                philosopher.HandleAction(_strategy.SelectAction(philosopher.Name, philosopher.LeftFork, philosopher.RightFork));
            }
        }
    }
}