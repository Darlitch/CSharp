using Model;

namespace IServices;

public interface ITableManager
{
    Fork GetFork(int index);
    int PhilosophersCount { get; }
}