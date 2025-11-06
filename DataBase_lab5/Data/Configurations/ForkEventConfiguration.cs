using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entity;

namespace Data.Configurations;

public class ForkEventConfiguration : IEntityTypeConfiguration<ForkEvent>
{
    public void Configure(EntityTypeBuilder<ForkEvent> builder)
    {
        builder.ToTable("fork_events");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();
        
        builder.Property(e => e.Index)
            .HasColumnName("index")
            .HasColumnType("integer");

        builder.Property(e => e.Owner)
            .HasColumnName("owner")
            .HasColumnType("varchar")
            .HasMaxLength(128);
        
        builder.Property(e => e.State)
            .HasColumnName("state")
            .HasColumnType("smallint");
        
        builder.Property(e => e.TimestampMs)
            .HasColumnName("timestamp_ms")
            .HasColumnType("bigint");
        
        builder.Property(e => e.RunId)
            .HasColumnName("run_id")
            .HasColumnType("bigint");
        
        builder.HasIndex(e => new {e.RunId, e.TimestampMs, e.Index}).IsUnique();
    }
}