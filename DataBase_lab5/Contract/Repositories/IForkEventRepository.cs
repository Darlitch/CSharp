using Model.Entity;

namespace Contract.Repositories;

public interface IForkEventRepository
{
    Task AddAsync(ForkEvent forkEvent, CancellationToken ct = default);
    Task<ForkEvent?> GetAsync(long runId, long timestampMs, int index, CancellationToken ct = default);
}