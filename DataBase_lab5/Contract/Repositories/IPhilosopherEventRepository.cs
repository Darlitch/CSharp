using Model.Entity;

namespace Contract.Repositories;

public interface IPhilosopherEventRepository
{
    public Task AddAsync(PhilosopherEvent philosopherEvent, CancellationToken ct = default);
    public Task<PhilosopherEvent?> GetAsync(long runId, long timestampMs, int index, CancellationToken ct = default);
}