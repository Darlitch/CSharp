using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entity;

namespace Data.Configurations;

public class SimulationRunConfiguration : IEntityTypeConfiguration<SimulationRun>
{
    public void Configure(EntityTypeBuilder<SimulationRun> builder)
    {
        builder.ToTable("simulation_run");
        builder.HasKey(s => s.RunId);
        
        builder.Property(s => s.RunId)
            .HasColumnName("run_id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.Duration)
            .HasColumnName("duration")
            .HasColumnType("bigint");
        
        builder.Property(s => s.PhilosophersCount)
            .HasColumnName("philosophers_count")
            .HasColumnType("integer");
    }
}