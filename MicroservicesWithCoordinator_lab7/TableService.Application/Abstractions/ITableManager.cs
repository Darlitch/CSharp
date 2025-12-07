using Application.Snapshots;
using Contract.Dtos;

namespace Application.Abstractions;

public interface ITableManager
{
    int PhilosophersCount { get; }
    void RegisterPhilosopher(RegisterPhilosopherDto dto);
    void UpdatePhilosopherMetrics(int id, PhilosopherMetricsDto metrics);
    void FinishPhilosopher(int id);
    TakeForkResult TryTakeLeftFork(int philosopherId);
    TakeForkResult TryTakeRightFork(int philosopherId);
    void ReleaseForks(int philosopherId);
    IReadOnlyList<ForkSnapshot> GetForksSnapshots();
    IReadOnlyList<MetricsSnapshot> GetMetricsSnapshots();
    bool IsReady();
    bool IsAllFinished();
    bool IsDeadLock();
}