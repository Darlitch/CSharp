using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Data;

public class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    public DbSet<SimulationRun> Simulations { get; set; }
    public DbSet<ForkEvent> ForkEvents { get; set; }
    public DbSet<PhilosopherEvent> PhilosopherEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}