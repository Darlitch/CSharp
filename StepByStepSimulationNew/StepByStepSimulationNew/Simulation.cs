using Model;
using Model.Enums;
using StepByStepSimulationNew.DTO;
using StrategyInterface;

namespace StepByStepSimulationNew;

public class Simulation
{
    private readonly IStrategy _strategy;
    private const int SimulationDuration = 1000000;
    private List<Fork> Forks { get; set; }
    private List<Philosopher> Philosophers { get; set; }

    public Simulation(IStrategy strategy)
    {
        Philosophers = PhilosopherInitializer.InitPhilosophers();
        Forks = Philosophers.Select(p => p.LeftFork).ToList();
        // Forks = Enumerable.Repeat(ForkState.Available, Philosophers.Count).ToList();
        _strategy = strategy;
    }

    public void Run()
    {
        for (int i = 0; i < SimulationDuration; ++i)
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
        for (int i = 0; i < Philosophers.Count; ++i)
        {
            Philosophers[i].Update();
            if (Philosophers[i].IsHungry && Philosophers[i].Action == PhilosopherAction.None)
            {
                HandleAction(_strategy.TryToStartEating(Forks[i], Forks[(i + 1) % Forks.Count], Philosophers[i].Name), i);
            }
        }
    }

    private void HandleAction(PhilosopherAction action, int philosopherId)
    {
        switch (action)
        {
            case PhilosopherAction.TakeLeftFork:
                Philosophers[philosopherId].TakeLeftFork();
                break;
            case PhilosopherAction.TakeRightFork:
                Philosophers[philosopherId].TakeRightFork();
                break;
            default:
                break;
        }
    }

    
}