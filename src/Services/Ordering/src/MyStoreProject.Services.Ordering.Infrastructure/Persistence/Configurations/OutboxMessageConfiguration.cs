using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyStoreProject.Services.Ordering.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OUTBOX_MESSAGES");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(m => m.EventId)
            .IsRequired();
        
        builder.Property(m => m.Topic)
            .IsRequired();

        builder.Property(m => m.Payload)
            .HasColumnType("json")
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasIndex(o => o.CreatedAt);


    }
}