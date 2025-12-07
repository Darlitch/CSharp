namespace Application.Abstractions;

public interface ISimulationTime
{
    long CurrentTimeMs { get; }
    void Start();
}