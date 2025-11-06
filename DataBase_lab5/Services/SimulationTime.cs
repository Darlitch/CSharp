using System.Diagnostics;
using Contract.Services;

namespace Services;

public class SimulationTime : ISimulationTime
{
    private readonly Stopwatch _stopwatch = new();
    
    public long CurrentTimeMs => _stopwatch.ElapsedMilliseconds;
    
    public void Start() => _stopwatch.Start();
    
    public void Stop() => _stopwatch.Stop();
}