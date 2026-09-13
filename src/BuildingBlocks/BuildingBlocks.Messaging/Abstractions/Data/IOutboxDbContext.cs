using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Messaging.Abstractions.Data;

public interface IOutboxDbContext
{
    public DbSet<OutboxMessage> OutboxMessages { get; }
}