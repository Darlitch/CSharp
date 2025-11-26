using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entity;

namespace Data.Configurations;

public class TestPhilosopherEventConfiguration : IEntityTypeConfiguration<PhilosopherEvent>
{
    public void Configure(EntityTypeBuilder<PhilosopherEvent> builder)
    {
        builder.ToTable("philosopher_event");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();
        
        builder.Property(e => e.Index)
            .HasColumnName("index")
            .HasColumnType("integer");
        
        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .HasColumnName("state")
            .HasColumnType("smallint");
        
        builder.Property(e => e.Action)
            .HasColumnName("action")
            .HasColumnType("smallint");
        
        builder.Property(e => e.Eaten)
            .HasColumnName("eaten")
            .HasColumnType("integer");
        
        builder.Property(e => e.WaitingTime)
            .HasColumnName("waiting_time")
            .HasColumnType("bigint");
        
        builder.Property(e => e.TimestampMs)
            .HasColumnName("timestamp_ms")
            .HasColumnType("bigint");
        
        builder.Property(e => e.RunId)
            .HasColumnName("run_id")
            .HasColumnType("integer");
        
        builder.HasIndex(e => new {e.RunId, e.TimestampMs, e.Index}).IsUnique();
    }
}