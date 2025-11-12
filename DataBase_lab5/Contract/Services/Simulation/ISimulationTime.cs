namespace Contract.Services.Simulation;

public interface ISimulationTime
{
    long CurrentTimeMs { get; }
}