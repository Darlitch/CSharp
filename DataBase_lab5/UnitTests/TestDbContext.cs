using Data;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace UnitTests;

public class TestDbContext(DbContextOptions<DataBaseContext> options) : DataBaseContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SimulationRun>(entity =>
        {
            entity.HasKey(e => e.RunId);
            entity.Property(s => s.RunId)
                .HasColumnType("integer")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<PhilosopherEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("integer")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.RunId)
                .HasColumnType("integer");
        });
        
        modelBuilder.Entity<ForkEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("integer")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.RunId)
                .HasColumnType("integer");
        });
    }
}