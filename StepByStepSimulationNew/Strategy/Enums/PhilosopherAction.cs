namespace StepByStepSimulationNew.Enums;

public enum PhilosopherAction
{
    TakeRightFork,
    TakeLeftFork,
    ReleaseRightFork, // зачем нам освобождения вилок, если действие моментальное?
    ReleaseLeftFork,
    None
}