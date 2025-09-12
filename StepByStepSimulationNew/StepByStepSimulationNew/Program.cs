// See https://aka.ms/new-console-template for more information

using StepByStepSimulationNew.Models;
using Strategy;
using Strategy.Strategies;
using StrategyInterface;

namespace StepByStepSimulationNew;

internal class Program
{
    public static void Main(string[] args)
    {
        IStrategy strategy = new NaiveStrategy();
        Simulation simulation = new Simulation(strategy);
        simulation.Run();
    }
}