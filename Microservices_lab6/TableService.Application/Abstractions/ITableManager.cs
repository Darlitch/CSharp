using Application.Snapshots;
using Contract.Dtos;
using TableService.Domain;

namespace Application.Abstractions;

public interface ITableManager
{
    int PhilosophersCount { get; }
    void RegisterPhilosopher(RegisterPhilosopherDto dto);
    void UpdatePhilosopherMetrics(int id, PhilosopherMetricsDto metrics);
    void FinishPhilosopher(int id);
    IReadOnlyList<ForkSnapshot> GetForksSnapshots();
    IReadOnlyList<MetricsSnapshot> GetMetricsSnapshots();
    bool IsAllFinished();
    bool IsDeadLock();
}