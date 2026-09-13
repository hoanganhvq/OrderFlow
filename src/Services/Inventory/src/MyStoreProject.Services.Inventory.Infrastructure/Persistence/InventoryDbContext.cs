using BuildingBlocks.Messaging.Abstractions.Data;
using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;
using MyStoreProject.Services.Inventory.Domain.Entities;

namespace MyStoreProject.Services.Inventory.Infrastructure.Persistence;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) 
    : DbContext(options), IInventoryDbContext, IOutboxDbContext, IInboxDbContext
{
    public DbSet<Domain.Entities.Inventory> Inventories => Set<Domain.Entities.Inventory>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
    }
}