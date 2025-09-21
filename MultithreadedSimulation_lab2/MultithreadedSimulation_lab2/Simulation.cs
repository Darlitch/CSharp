using Model;
using Model.Enums;
using MultithreadedSimulation_lab2.DTO;
using StrategyInterface;

namespace MultithreadedSimulation_lab2;

public class Simulation
{
    private readonly IStrategy _strategy;
    private const int SimulationDuration = 1000000;
    // private const int SimulationDuration = 300;
    private List<Fork> Forks { get; set; }
    private List<Philosopher> Philosophers { get; set; }

    public Simulation(IStrategy strategy, List<Philosopher> philosophers)
    {
        Philosophers = philosophers;
        Forks = Philosophers.Select(p => p.LeftFork).ToList();
        _strategy = strategy;
    }

    public void Run()
    {
        for (var i = 0; i < SimulationDuration; ++i)
        {
            RunStep();
            if (i % 1000 == 0 && i != 0)
            {
                Metrics.PrintMetrics(new MetricDto(Philosophers, Forks, i));
            }
            if (Philosophers.All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) && Forks.All(f => f.State == ForkState.InUse))
            {
                Metrics.PrintFinalMetrics(new MetricDto(Philosophers, Forks, i));
                Console.WriteLine($"Deadlock at step {i}!");
                return;
            }
        }
        Metrics.PrintMetrics(new MetricDto(Philosophers, Forks, SimulationDuration));
    }

    private void RunStep()
    {
        for (var i = 0; i < Philosophers.Count; ++i)
        {
            Philosophers[i].Update();
            if (Philosophers[i].IsHungry && Philosophers[i].Action == PhilosopherAction.None)
            {
                Philosophers[i].HandleAction(_strategy.SelectAction(Philosophers[i].Name, Forks[i], Forks[(i + 1) % Forks.Count]));
            }
        }
    }
}