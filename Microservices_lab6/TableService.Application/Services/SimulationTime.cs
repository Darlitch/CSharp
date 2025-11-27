using System.Diagnostics;
using Application.Abstractions;

namespace Application.Services;

public class SimulationTime : ISimulationTime
{
    private readonly Stopwatch _stopwatch = new();
    
    public void Start()
    {
        _stopwatch.Start();
    }
    
    public long CurrentTimeMs => _stopwatch.ElapsedMilliseconds;
}