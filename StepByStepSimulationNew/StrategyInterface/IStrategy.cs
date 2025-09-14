using Model;
using Model.Enums;

namespace StrategyInterface;

public interface IStrategy
{
    public PhilosopherAction TryToStartEating(Fork leftFork, Fork rightFork, string name);
}