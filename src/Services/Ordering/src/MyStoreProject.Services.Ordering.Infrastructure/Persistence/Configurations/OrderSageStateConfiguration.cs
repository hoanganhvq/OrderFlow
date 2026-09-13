using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyStoreProject.Services.Ordering.Domain.Entities;

namespace MyStoreProject.Services.Ordering.Infrastructure.Persistence.Configurations;

public class OrderSageStateConfiguration : IEntityTypeConfiguration<OrderSagaState>
{
    public void Configure(EntityTypeBuilder<OrderSagaState> builder)
    {
        builder.ToTable("ORDER_SAGA_STATES");
        
        builder.HasKey(o => o.OrderId);
        
        builder.Property(s => s.ReservationCompleted)
            .IsRequired();

        builder.Property(s => s.PaymentCompleted)
            .IsRequired();
        
        builder.HasOne<Order>()
            .WithOne()
            .HasForeignKey<OrderSagaState>(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}