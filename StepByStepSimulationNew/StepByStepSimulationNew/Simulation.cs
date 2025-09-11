using StepByStepSimulationNew.Enums;
using StepByStepSimulationNew.Models;
using Strategy;

namespace StepByStepSimulationNew;

public class Simulation
{
    private readonly IStrategy _strategy;
    private const int SimulationDuration = 1000000;
    public List<ForkState> Forks { get; set; }
    public List<Philosopher> Philosophers { get; set; }

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
        }
    }

    public void RunStep()
    {
        for (int i = 0; i < Philosophers.Count; ++i)
        {
            if (Philosophers[i].IsHungry)
            {
                
            }
            Philosophers[i].Update();
        }
    }
}