using StepByStepSimulationNew.Enums;
using StepByStepSimulationNew.Models;
using StepByStepSimulationNew.Models.DTO;
using StrategyInterface;

namespace StepByStepSimulationNew;

public class Simulation
{
    private readonly IStrategy _strategy;
    private const int SimulationDuration = 1000000;
    private List<ForkState> Forks { get; set; }
    private List<Philosopher> Philosophers { get; set; }

    public Simulation(IStrategy strategy)
    {
        Philosophers = PhilosopherInitializer.InitPhilosophers();
        Forks = Enumerable.Repeat(ForkState.Available, Philosophers.Count).ToList();
        _strategy = strategy;
    }

    public void Run()
    {
        for (int i = 0; i < SimulationDuration; ++i)
        {
            RunStep();
            if (i % 1 == 1000 && i != 0)
            {
                Metrics.PrintMetrics(new MetricDto(Philosophers, Forks, i));
            }
            if (Philosophers.All(p => p is { IsHungry: true, Action: PhilosopherAction.None }) && Forks.All(f => f == ForkState.InUse))
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
                PhilosopherAction currAction = _strategy.TryToStartEating(Forks[i], Forks[(i + 1) % Forks.Count]);
                HandleAction(currAction, i);
            }
            
            
            
            if (Philosophers[i].Action == PhilosopherAction.ReleaseForks)
            {
                Forks[i] = ForkState.Available;
                Forks[(i + 1) % Forks.Count] = ForkState.Available;
                Philosophers[i].ReleaseFork();
            }
        }
    }

    private void HandleAction(PhilosopherAction action, int philosopherId)
    {
        switch (action)
        {
            case PhilosopherAction.TakeLeftFork:
                Forks[philosopherId] = ForkState.InUse;
                Philosophers[philosopherId].TakeLeftFork();
                break;
            case PhilosopherAction.TakeRightFork:
                Philosophers[philosopherId].TakeRightFork();
                Forks[(philosopherId + 1) % Forks.Count] = ForkState.InUse;
                break;
            default:
                break;
        }
    }

    
}