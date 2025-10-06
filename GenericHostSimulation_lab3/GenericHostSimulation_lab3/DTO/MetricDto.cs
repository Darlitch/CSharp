using Model;
using PhilosopherService;

namespace GenericHostSimulation_lab3.DTO;

public class MetricDto(List<PhilosopherHostedService> philosophers, List<Fork> forks, long currTime)
{
    public List<PhilosopherHostedService> Philosophers { get; } = philosophers;
    public List<Fork> Forks { get; } = forks;
    public long CurrTime { get; } = currTime;
}