using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy.Strategies;

public class CoordinatorStrategy(Coordinator coordinator) : IStrategy
{
    private Coordinator _coordinator = coordinator;

    public PhilosopherAction SelectAction(string name, Fork leftFork, Fork rightFork)
    {
        if (leftFork.State == ForkState.Available)
        {
            coordinator.TakeLeftFork(name);
        }
        else
        {
            coordinator.Update();
        }
        return PhilosopherAction.None;
    }
    
}