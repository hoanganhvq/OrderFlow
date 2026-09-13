using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyStoreProject.Services.Ordering.Domain.Entities;


namespace MyStoreProject.Services.Ordering.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("ORDER_ITEMS");
        builder.HasKey(o => o.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(ol => ol.Sku)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ol => ol.Quantity)
            .IsRequired();

        builder.Property(ol => ol.UnitPrice)
            .HasPrecision(10, 2)
            .IsRequired();
        
        
    }
}