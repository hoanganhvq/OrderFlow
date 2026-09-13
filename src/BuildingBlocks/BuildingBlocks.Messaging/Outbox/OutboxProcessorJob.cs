using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.Outbox;

public class OutboxProcessorJob<TContext> : BackgroundService where TContext : DbContext, IOutboxDbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<OutboxProcessorJob<TContext>> _logger;
    
    public OutboxProcessorJob(IServiceProvider serviceProvider,
        IEventPublisher eventPublisher,
        ILogger<OutboxProcessorJob<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessageAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }
    }

    private async Task ProcessOutboxMessageAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        
        var messages = await context.OutboxMessages
            .Where(om => om.PublishedAt  == null)
            .OrderBy(om => om.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
        {
            return;
        }

        foreach (var outboxMessage in messages)
        {
            await _eventPublisher.PublishAsync(
                outboxMessage.Topic,
                outboxMessage.Payload,
                cancellationToken);
            outboxMessage.MarkAsPublished();
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}