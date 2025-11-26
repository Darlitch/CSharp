using Application.Abstractions;
using Application.Configuration;
using Application.Extensions;
using Application.Snapshots;
using Contract.Dtos;
using Microsoft.Extensions.Options;
using TableService.Domain;
using TableService.Domain.Enums;

namespace Application.Services;

public class TableManager : ITableManager
{
    private readonly List<Fork> _forks;
    private readonly Dictionary<int, PhilosopherEntry> _philosophers;
    private readonly Lock _lock;
    public int PhilosophersCount { get; }

    public TableManager(IOptions<TableServiceOptions> options)
    {
        PhilosophersCount = options.Value.PhilosophersCount;
        _forks = new List<Fork>();
        for (var i = 0; i < PhilosophersCount; ++i)
        {
            _forks.Add(new Fork());
        }
        _philosophers = new Dictionary<int, PhilosopherEntry>();
        _lock = new Lock();
    }

    public void RegisterPhilosopher(RegisterPhilosopherDto dto)
    {
        using var _ = _lock.EnterScope();
        if (!_philosophers.ContainsKey(dto.Id))
        {
            _philosophers.Add(dto.Id, dto.ToPhilosopherEntry());
        }
    }

    public void UpdatePhilosopherMetrics(int id, PhilosopherMetricsDto metrics)
    {
        using var _ = _lock.EnterScope();
        if (_philosophers.TryGetValue(id, out var entry))
        {
            entry.Metrics = metrics;
        }
    }

    public void FinishPhilosopher(int id)
    {
        using var _ = _lock.EnterScope();
        if (_philosophers.TryGetValue(id, out var entry))
        {
            entry.IsFinished = true;
        }
    }

    public IReadOnlyList<ForkSnapshot> GetForksSnapshots()
    {
        using var _ = _lock.EnterScope();
        return _forks.Select(f => new ForkSnapshot(f.State, f.Owner, f.FreeTime, f.BlockTime)).ToList();
    }

    public IReadOnlyList<MetricsSnapshot> GetMetricsSnapshots()
    {
        using var _ = _lock.EnterScope();
        return _philosophers.Values
            .Where(e => e.Metrics is not null)
            .OrderBy(e => e.Id)
            .Select(e =>
            {
                var m = e.Metrics!;
                return new MetricsSnapshot(e.Id, e.Name, m.State, m.Action,
                    m.CurrentActionDuration, m.Eaten, m.WaitingTime);
            }).ToList();
    }
    
    public bool IsAllFinished()
    {
        using var _ = _lock.EnterScope();
        return _philosophers.Values.All(p => p.IsFinished);
    }

    public bool IsDeadLock()
    {
        using var _ = _lock.EnterScope();
        if (_forks.Any(f => f.Owner is null))
        {
            return false;
        }
        var owners = new HashSet<string>();
        foreach (var fork in _forks)
        {
            if (!owners.Add(fork.Owner!))
            {
                return false;
            }
        }
        return true;
    }
}