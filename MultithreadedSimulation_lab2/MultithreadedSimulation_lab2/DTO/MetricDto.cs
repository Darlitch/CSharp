using Model;

namespace MultithreadedSimulation_lab2.DTO;

public class MetricDto(List<Philosopher> philosophers, List<Fork> forks, int steps)
{
    public List<Philosopher> Philosophers { get; } = philosophers;
    public List<Fork> Forks { get; } = forks;
    public int Steps { get; } = steps;
}