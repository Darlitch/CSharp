using Model.Entity;

namespace Contract.Repositories;

public interface ISimulationRunRepository
{
    Task<long> AddAsync(SimulationRun simulation, CancellationToken ct = default);
    Task UpdateAsync(long runId, long durationMs, CancellationToken ct = default);
    Task<SimulationRun?> GetAsync(long runId, CancellationToken ct = default);
}