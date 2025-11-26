using Contract.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Data.Repositories;

public class ForkEventRepository(DataBaseContext context) : IForkEventRepository
{
    public async Task AddAsync(ForkEvent forkEvent, CancellationToken ct = default)
    {
        await context.ForkEvents.AddAsync(forkEvent, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ForkEvent?> GetAsync(long runId, long timestampMs, int index, CancellationToken ct = default)
    {
        return await context.ForkEvents
            .AsNoTracking()
            .Where(e => e.Index == index && e.RunId == runId && e.TimestampMs <= timestampMs)
            .OrderByDescending(e => e.TimestampMs)
            .FirstOrDefaultAsync(ct);
    }
}