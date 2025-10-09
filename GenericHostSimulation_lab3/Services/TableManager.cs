using System.Xml;
using IServices;
using Model;
using Model.Enums;

namespace Services;

public class TableManager : ITableManager
{
    private readonly List<Fork> _forks;

    public TableManager(int philosophersCount)
    {
        _forks = new List<Fork>();
        for (var i = 0; i < philosophersCount; ++i)
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

    public int PhilosophersCount => _forks.Count;
}