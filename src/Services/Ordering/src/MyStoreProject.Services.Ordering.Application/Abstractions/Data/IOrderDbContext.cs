using BuildingBlocks.Messaging.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Domain.Entities;

namespace MyStoreProject.Services.Ordering.Application.Abstractions.Data;

public interface IOrderDbContext : IOutboxDbContext, IInboxDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderLines { get; }
    DbSet<OrderSagaState> OrderSagaStates { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}