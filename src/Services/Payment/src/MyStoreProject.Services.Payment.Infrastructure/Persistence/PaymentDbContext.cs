using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Payment.Application.Abstractions.Data;

namespace MyStoreProject.Services.Payment.Infrastructure.Persistence;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) 
    : DbContext(options), IPaymentDbContext
{
    public DbSet<Domain.Entities.Payment>  Payments => Set<Domain.Entities.Payment>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
    }
}