using Contract.Repositories;
using Model.Entity;

namespace Data.Repositories;

public class SimulationRunRepository(DataBaseContext context) : ISimulationRunRepository
{
    public async Task<long> AddAsync(SimulationRun simulation, CancellationToken ct = default)
    {
        await context.Simulations.AddAsync(simulation, ct);
        await context.SaveChangesAsync(ct);
        return simulation.RunId;
    }
}