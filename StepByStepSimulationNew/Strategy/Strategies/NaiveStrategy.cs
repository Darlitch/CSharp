using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy.Strategies;

public class NaiveStrategy : IStrategy
{
    
    public PhilosopherAction TryToStartEating(Fork leftFork, Fork rightFork, string name)
    {
        if (leftFork.State == ForkState.Available)
        {
            return PhilosopherAction.TakeLeftFork;
        }
        if (rightFork.State == ForkState.Available && leftFork.Owner == name)
        {
            return PhilosopherAction.TakeRightFork;
        }
        return PhilosopherAction.None;
    }
}