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

    public Fork GetFork(int index) => _forks[(index + 1) % _forks.Count];

    public bool AllInUse()
    {
        return _forks.All(t => t.State == ForkState.InUse);
    }

    public int PhilosophersCount => _forks.Count;
}