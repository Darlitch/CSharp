using Model.Entity;

namespace Contract.Repositories;

public interface ISimulationRunRepository
{
    public Task<long> AddAsync(SimulationRun simulation, CancellationToken ct = default);
}