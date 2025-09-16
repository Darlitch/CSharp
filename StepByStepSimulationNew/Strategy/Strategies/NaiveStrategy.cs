using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy.Strategies;

public class NaiveStrategy : IStrategy
{
    
    public PhilosopherAction SelectAction(string name, Fork leftFork, Fork rightFork)
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