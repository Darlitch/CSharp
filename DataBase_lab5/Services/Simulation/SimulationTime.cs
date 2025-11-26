using System.Diagnostics;
using Contract.Services.Simulation;

namespace Services.Simulation;

public class SimulationTime : ISimulationTime
{
    public SimulationTime()
    {
        _stopwatch.Start();
    }
    private readonly Stopwatch _stopwatch = new();
    
    public long CurrentTimeMs => _stopwatch.ElapsedMilliseconds;
}