using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyStoreProject.Services.Payment.Infrastructure.Persistence.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Domain.Entities.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Payment> builder)
    {
        builder.ToTable("PAYMENTS");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderId)
            .IsRequired();
        
        builder.HasIndex(p => p.OrderId)
            .IsUnique();
        
        builder.Property(p=>p.Status)
            .HasConversion<string>()
            .IsRequired();
        builder.Property(p=>p.Amount)
            .HasPrecision(10, 2)
            .IsRequired();
        
    }
}