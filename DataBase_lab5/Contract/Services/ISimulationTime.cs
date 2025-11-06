namespace Contract.Services;

public interface ISimulationTime
{
    public long CurrentTimeMs { get; }
    void Start();
    void Stop();
}