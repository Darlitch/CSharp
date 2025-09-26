using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy;

public class NaiveStrategy : IStrategy
{
    public PhilosopherAction SelectAction(string name, Fork leftFork, Fork rightFork)
    {
        if (leftFork.IsAvailable())
        {
            return PhilosopherAction.TakeLeftFork;
        }
        if (rightFork.IsAvailable() && leftFork.IsOwner(name))
        {
            return PhilosopherAction.TakeRightFork;
        }
        return PhilosopherAction.None;
    }
}