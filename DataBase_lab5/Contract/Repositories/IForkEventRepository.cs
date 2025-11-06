using Model.Entity;

namespace Contract.Repositories;

public interface IForkEventRepository
{
    public Task AddAsync(ForkEvent forkEvent, CancellationToken ct = default);
    public Task<ForkEvent?> GetAsync(long runId, long timestampMs, int index, CancellationToken ct = default);
}