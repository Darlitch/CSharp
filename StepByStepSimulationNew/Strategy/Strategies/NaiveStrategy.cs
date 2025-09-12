using StepByStepSimulationNew.Enums;
using StrategyInterface;

namespace Strategy.Strategies;

public class NaiveStrategy : IStrategy
{
    
    public PhilosopherAction TryToStartEating(ForkState leftFork, ForkState rightFork)
    {
        if (leftFork == ForkState.Available)
        {
            return PhilosopherAction.TakeLeftFork;
        }
        if (rightFork == ForkState.Available)
        {
            return PhilosopherAction.TakeRightFork;
        }
        return PhilosopherAction.None;
    }
}