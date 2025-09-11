using System;
using System.Collections.Generic;
using System.Linq;
using StepByStepSimulationNew.Enums;
using StepByStepSimulationNew.Models;
using Strategy;

namespace StepByStepSimulationNew;

public class Simulation
{
    private readonly IStrategy _strategy;
    private const int SimulationDuration = 1000000;
    private List<ForkState> Forks { get; set; }
    private List<Philosopher> Philosophers { get; set; }

    public Simulation(IStrategy strategy)
    {
        Forks = Enumerable.Repeat(ForkState.Available, 5).ToList();
        Philosophers = PhilosopherInitializer.InitPhilosophers();
        _strategy = strategy;
    }

    public void Run()
    {
        for (int i = 0; i < SimulationDuration; ++i)
        {
            RunStep();
            if (Philosophers.All(p => p.IsHungry) && Forks.All(f => f == ForkState.InUse))
            {
                Console.WriteLine($"Deadlock at step {i}!");
                break;
            }
        }
    }

    private void RunStep()
    {
        for (int i = 0; i < Philosophers.Count; ++i)
        {
            if (Philosophers[i].IsHungry && Philosophers[i].Action == PhilosopherAction.None)
            {
                PhilosopherAction currAction = _strategy.TryToStartEating(Forks[i], Forks[(i + 1) % Forks.Count]);
                HandleAction(currAction, i);
            }
            Philosophers[i].Update();
            
            
            
            if (Philosophers[i].Action == PhilosopherAction.ReleaseLeftFork)
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
            case PhilosopherAction.None:
            case PhilosopherAction.ReleaseRightFork:
            case PhilosopherAction.ReleaseLeftFork: // тут немного чудит райдер с подсказками, поэтому вопрос в реализации
            default:
                break;
        }
    }

    
}