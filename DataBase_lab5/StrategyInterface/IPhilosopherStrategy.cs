using Model;
using Model.Enums;

namespace StrategyInterface;

public interface IPhilosopherStrategy
{
    public PhilosopherAction SelectAction(string name, Fork leftFork, Fork rightFork);
}