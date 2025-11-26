using Contract.Services;
using Microsoft.Extensions.Options;
using Model;
using Model.Enums;
using Services.Simulation;

namespace Services;

public class TableManager : ITableManager
{
    private readonly List<Fork> _forks;
    public int PhilosophersCount { get; }

    public TableManager(IOptions<SimulationOptions> options)
    {
        PhilosophersCount = options.Value.PhilosophersCount;
        _forks = new List<Fork>();
        for (var i = 0; i < PhilosophersCount; ++i)
        {
            _forks.Add(new Fork());
        }
    }

    public Fork GetFork(int index)
    {
        // Console.WriteLine($"{index} : {index % _forks.Count}");
        return _forks[index % PhilosophersCount];
    }

    public bool AllInUse()
    {
        return _forks.All(t => t.State == ForkState.InUse);
    }

}