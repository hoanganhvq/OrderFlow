using BuildingBlocks.Messaging.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Payment.Domain.Entities;

namespace MyStoreProject.Services.Payment.Application.Abstractions.Data;

public interface IPaymentDbContext : IOutboxDbContext, IInboxDbContext
{
    DbSet<Domain.Entities.Payment> Payments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);   
}