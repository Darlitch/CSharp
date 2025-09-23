using Model;

namespace MultithreadedSimulation_lab2.DTO;

public class MetricDto(List<Philosopher> philosophers, List<Fork> forks, long currTime)
{
    public List<Philosopher> Philosophers { get; } = philosophers;
    public List<Fork> Forks { get; } = forks;
    public long CurrTime { get; } = currTime;
}