using StepByStepSimulationNew.Enums;

namespace Strategy;

public interface IStrategy
{
    public PhilosopherAction TryToStartEating(ForkState leftFork, ForkState rightFork);
}