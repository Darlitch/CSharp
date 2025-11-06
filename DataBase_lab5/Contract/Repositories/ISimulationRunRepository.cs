using Model.Entity;

namespace Contract.Repositories;

public interface ISimulationRunRepository
{
    public Task<SimulationRun> AddAsync(SimulationRun simulation, CancellationToken ct = default);
}