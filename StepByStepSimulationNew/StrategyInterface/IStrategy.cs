using StepByStepSimulationNew.Enums;

namespace StrategyInterface;

public interface IStrategy
{
    public PhilosopherAction TryToStartEating(ForkState leftFork, ForkState rightFork);
}