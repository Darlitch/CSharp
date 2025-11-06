using Contract.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Data.Repositories;

public class PhilosopherEventRepository(DataBaseContext context) : IPhilosopherEventRepository
{
    public async Task AddAsync(PhilosopherEvent philosopherEvent, CancellationToken ct = default)
    {
        await context.PhilosopherEvents.AddAsync(philosopherEvent, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<PhilosopherEvent?> GetAsync(long runId, long timestampMs, int index,
        CancellationToken ct = default)
    {
        return await context.PhilosopherEvents
            .Where(e => e.RunId == runId && e.TimestampMs <= timestampMs && e.Index == index)
            .OrderByDescending(e => e.TimestampMs)
            .FirstOrDefaultAsync(ct);
    }
}