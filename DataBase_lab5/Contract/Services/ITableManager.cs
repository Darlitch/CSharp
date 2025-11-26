using Model;

namespace Contract.Services;

public interface ITableManager
{
    Fork GetFork(int index);
    int PhilosophersCount { get; }
    bool AllInUse();
}