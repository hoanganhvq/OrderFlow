using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyStoreProject.Services.Inventory.Domain.Entities;

namespace MyStoreProject.Services.Inventory.Infrastructure.Persistence.Configuration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("RESERVATIONS");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Sku)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(r => new { r.OrderId, r.Sku })
            .IsUnique();
        
        builder.Property(r => r.RowVersion)
            .IsRowVersion()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");;
    }
}