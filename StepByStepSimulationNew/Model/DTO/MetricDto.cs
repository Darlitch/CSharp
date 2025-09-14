using StepByStepSimulationNew.Enums;

namespace StepByStepSimulationNew.Models.DTO;

public class MetricDto(List<Philosopher> philosophers, List<ForkState> forks, int steps)
{
    public List<Philosopher> Philosophers { get; } = philosophers;
    public List<ForkState> Forks { get; } = forks;
    public int Steps { get; } = steps;
}