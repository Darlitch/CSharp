using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entity;

namespace Data.Configurations;

public class TestSimulationRunConfiguration : IEntityTypeConfiguration<SimulationRun>
{
    public void Configure(EntityTypeBuilder<SimulationRun> builder)
    {
        builder.ToTable("simulation_run");
        builder.HasKey(s => s.RunId);
        
        builder.Property(s => s.RunId)
            .HasColumnName("run_id")
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.DurationMs)
            .HasColumnName("duration_ms")
            .HasColumnType("bigint");
        
        builder.Property(s => s.PhilosophersCount)
            .HasColumnName("philosophers_count")
            .HasColumnType("integer");
    }
}