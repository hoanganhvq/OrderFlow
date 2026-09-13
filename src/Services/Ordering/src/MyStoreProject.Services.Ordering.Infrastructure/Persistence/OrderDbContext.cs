using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;
using MyStoreProject.Services.Ordering.Domain.Entities;

namespace MyStoreProject.Services.Ordering.Infrastructure.Persistence;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) 
    : DbContext(options), IOrderDbContext
{
    
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderLines { get; }
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage>  InboxMessages => Set<InboxMessage>();
    public DbSet<OrderSagaState> OrderSagaStates => Set<OrderSagaState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }
    
}