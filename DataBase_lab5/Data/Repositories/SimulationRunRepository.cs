using Contract.Repositories;
using Microsoft.EntityFrameworkCore;
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

    public async Task UpdateAsync(long runId, long durationMs, CancellationToken ct = default)
    {
        var run = await context.Simulations.FindAsync(runId, ct);
        run?.UpdateDuration(durationMs);
    }

    public async Task<bool> ExistsAsync(long runId, CancellationToken ct = default)
    {
        var run = await context.Simulations.AsNoTracking().FirstOrDefaultAsync(s => s.RunId == runId, ct);
        return run != null;
    }
}