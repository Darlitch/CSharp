using Model;
using Model.Enums;
using StrategyInterface;

namespace Strategy.Strategies;

public class CoordinatorStrategy(ICoordinator coordinator) : IStrategy
{
    private readonly ICoordinator _coordinator = coordinator;

    public PhilosopherAction SelectAction(string name, Fork leftFork, Fork rightFork)
    {
        if (leftFork.State == ForkState.Available)
        {
            _coordinator.TakeLeftFork(name);
        }
        else
        {
            _coordinator.Update();
        }
        return PhilosopherAction.None;
    }
    
}