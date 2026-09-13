using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyStoreProject.Services.Inventory.Infrastructure.Persistence.Configuration;

public class InventoryConfiguration : IEntityTypeConfiguration<Domain.Entities.Inventory>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Inventory> builder)
    {
        builder.ToTable("INVENTORY");
        builder.HasKey(i => i.Sku);
        builder.Property(i => i.QuantityOnHand)
            .IsRequired();
        builder.Property(i=>i.QuantityReserved)
            .IsRequired();

        var item1 = Domain.Entities.Inventory. Create("WIDGET-01", 20).Value;
        var item2 = Domain.Entities.Inventory.Create("WIDGET-02", 30).Value;
        var item3 = Domain.Entities.Inventory.Create("WIDGET-03", 2).Value; 

        builder.HasData(item1, item2, item3);
        
    }
}