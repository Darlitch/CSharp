using Model;
using Strategy;
using Strategy.Strategies;
using StrategyInterface;

namespace StepByStepSimulationNew;

internal static class Program
{
    public static void Main(string[] args)
    {
        List<Philosopher> philosophers = PhilosopherInitializer.InitPhilosophers();
        List<Fork> forks = philosophers.Select(p => p.LeftFork).ToList();
        
        // IStrategy strategy = new NaiveStrategy();
        // Simulation simulation = new Simulation(strategy, philosophers);
        // simulation.Run();
        
        ICoordinator coordinator = new Coordinator(philosophers, forks);
        IStrategy strategy2 = new CoordinatorStrategy(coordinator);
        Simulation simulation2 = new Simulation(strategy2, philosophers);
        simulation2.Run();
    }
}