using System.Buffers;
using System.Text;
using System.Text.Json;
using BuildingBlocks.Messaging.Abstractions.Data;
using BuildingBlocks.Messaging.Entities;
using BuildingBlocks.Messaging.Helper;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.Inbox;

public abstract class BaseEventConsumer<TContext, TEvent> : BackgroundService
    where TContext : DbContext, IInboxDbContext
{
    private const string DeadLetterTopic = "DeadLetterTopic";
    private const int MaxRetryAttempts = 3;
    
    private readonly IPulsarClient _pulsarClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _topic;
    private readonly string _subscription;
    private readonly ILogger<BaseEventConsumer<TContext, TEvent>> _logger;
    
    protected abstract Task<bool> HandleEventAsync(TEvent @event, IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default);
    
    public BaseEventConsumer(IServiceProvider serviceProvider,
        IPulsarClient pulsarClient,
        ILogger<BaseEventConsumer<TContext, TEvent>> logger,
        string topic,
        string subscription)
    {
        _serviceProvider = serviceProvider;
        _pulsarClient = pulsarClient;
        _logger = logger;
        _topic =  topic;
        _subscription = subscription;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var deadLetterProducer = _pulsarClient.NewProducer()
            .Topic(DeadLetterTopic)
            .Create();

        await using var retryProducer = _pulsarClient.NewProducer()
            .Topic(_topic)
            .Create();
        
        await using var consumer = _pulsarClient.NewConsumer()
            .Topic(_topic)
            .SubscriptionName(_subscription)
            .SubscriptionType(SubscriptionType.KeyShared)
            .Create();
        
        await foreach (var message in consumer.Messages(stoppingToken)) 
        {
            var retryCount = 0;
            if (message.Properties.TryGetValue("X-Retry-Count", out var retryCountString))
            {
                int.TryParse(retryCountString, out retryCount);
            }
            try
            {
                var payloadString = Encoding.UTF8.GetString(message.Data.ToArray());
                var payloadEvent = JsonSerializer.Deserialize<TEvent>(payloadString, 
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                var eventIdProperty = typeof(TEvent).GetProperty("EventId");
                if (eventIdProperty?.GetValue(payloadEvent) is not Guid eventId)
                {
                    throw new InvalidOperationException($"Event {typeof(TEvent).Name} is missing 'EventId' property.");
                }
                    
                if (payloadEvent is not null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<TContext>();
                
                    var alreadyProcessed = await context.InboxMessages
                        .AnyAsync(im => im.EventId == eventId, stoppingToken);

                    if (alreadyProcessed)
                    {
                        await consumer.Acknowledge(message, stoppingToken);
                        continue;
                    }
                
                    var isSuccess = await HandleEventAsync(payloadEvent, scope.ServiceProvider, stoppingToken);
                    
                    if (!isSuccess)
                    {
                        throw new InvalidOperationException("Failed to process event.");
                    }
                    await consumer.Acknowledge(message, stoppingToken);
                }
            }
            catch (Exception exception)
            {
                if (retryCount >= MaxRetryAttempts)
                {
                    await deadLetterProducer.NewMessage()
                        .Property("MessageId", message.MessageId.ToString())
                        .Property("Exception", exception.Message)
                        .Send(message.Data, stoppingToken);

                    await consumer.Acknowledge(message, stoppingToken);
                }
                else
                {
                    retryCount++;
                        
                    var delay = RetryPolicyHelper.CaculateBackoffDelay(retryCount);
                    await Task.Delay(delay, stoppingToken);
                        
                    var metadata = new MessageMetadata();
                    foreach (var prop in message.Properties)
                    {
                        metadata[prop.Key] = prop.Value;
                    }
                    metadata["X-Retry-Count"] = retryCount.ToString();  
                    
                    await retryProducer.Send(metadata, message.Data, stoppingToken);
                    await consumer.Acknowledge(message, stoppingToken); 
                }
            }
        }
    }
}