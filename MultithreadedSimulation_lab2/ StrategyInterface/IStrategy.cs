using Model;
using Model.Enums;

namespace StrategyInterface;

public interface IStrategy
{
    public PhilosopherAction SelectAction(string name, Fork leftFork, Fork rightFork);
}