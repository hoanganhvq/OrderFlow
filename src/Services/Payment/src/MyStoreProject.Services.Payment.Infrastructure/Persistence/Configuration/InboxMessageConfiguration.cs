using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyStoreProject.Services.Payment.Domain.Entities;

namespace MyStoreProject.Services.Payment.Infrastructure.Persistence.Configuration;

public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("INBOX_MESSAGES");

        builder.HasKey(i => i.EventId);
        
        builder.Property(m => m.ProcessedAt)
            .IsRequired();
        
    }
}