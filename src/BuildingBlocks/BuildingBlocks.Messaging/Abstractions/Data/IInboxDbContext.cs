using BuildingBlocks.Messaging.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Messaging.Abstractions.Data;

public interface IInboxDbContext
{
    public DbSet<InboxMessage> InboxMessages { get; }
}