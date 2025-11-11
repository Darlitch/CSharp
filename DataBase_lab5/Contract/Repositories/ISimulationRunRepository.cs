using Model.Entity;

namespace Contract.Repositories;

public interface ISimulationRunRepository
{
    public Task<long> AddAsync(SimulationRun simulation, CancellationToken ct = default);
    public Task UpdateAsync(long runId, long durationMs, CancellationToken ct = default);
    public Task<bool> ExistsAsync(long runId, CancellationToken ct = default);
}