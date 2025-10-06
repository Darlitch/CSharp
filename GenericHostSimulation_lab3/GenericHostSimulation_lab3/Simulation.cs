using System.Diagnostics;
using GenericHostSimulation_lab3.DTO;
using Model;
using Model.Enums;
using PhilosopherService;
using StrategyInterface;

namespace GenericHostSimulation_lab3;

public class Simulation
{
    private readonly IPhilosopherStrategy _philosopherStrategy;
    private const long SimulationDuration = 100000;
    private List<Fork> Forks { get; set; }
    private List<PhilosopherHostedService> Philosophers { get; set; }
    private readonly Stopwatch _stopwatch; 

    public Simulation(IPhilosopherStrategy philosopherStrategy, List<PhilosopherHostedService> philosophers)
    {
        Philosophers = philosophers;
        Forks = Philosophers.Select(p => p.LeftFork).ToList();
        _philosopherStrategy = philosopherStrategy;
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

    private void RunPhilosopher(PhilosopherHostedService philosopherHostedService)
    {
        while (_stopwatch.ElapsedMilliseconds < SimulationDuration)
        {
            philosopherHostedService.Update();
            if (philosopherHostedService is { IsHungry: true, Action: PhilosopherAction.None })
            {
                philosopherHostedService.HandleAction(_philosopherStrategy.SelectAction(philosopherHostedService.Name, philosopherHostedService.LeftFork, philosopherHostedService.RightFork));
            }
        }
    }
}