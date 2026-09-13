using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyStoreProject.Services.Payment.Domain.Entities;

namespace MyStoreProject.Services.Payment.Infrastructure.Persistence.Configuration;


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