using BuildingBlocks.Messaging.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace MyStoreProject.Services.Inventory.Application.Abstractions.Data;

public interface IInventoryDbContext : IOutboxDbContext, IInboxDbContext
{
    public DbSet<Domain.Entities.Reservation> Reservations { get;  }
    public DbSet<Domain.Entities.Inventory> Inventories { get;  }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}