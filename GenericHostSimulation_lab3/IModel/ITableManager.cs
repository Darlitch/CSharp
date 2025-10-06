namespace IModel;

public interface ITableManager
{
    IFork GetFork(int index);
    int PhilosophersCount { get; }
}