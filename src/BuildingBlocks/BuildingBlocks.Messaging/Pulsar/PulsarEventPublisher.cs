using System.Buffers;
using System.Text;
using System.Text.Json;
using BuildingBlocks.Messaging.Abstractions;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.Pulsar;

public class PulsarEventPublisher : IEventPublisher
{
    private readonly IPulsarClient _pulsarClient;
    private readonly ILogger<PulsarEventPublisher> _logger;
    private readonly Dictionary<string, IProducer<ReadOnlySequence<byte>>> _producers = new();

    
    public PulsarEventPublisher(IPulsarClient pulsarClient,
        ILogger<PulsarEventPublisher> logger)
    {
        _pulsarClient = pulsarClient;
        _logger = logger;
    }
    
    public async Task PublishAsync(string topic, string payload, CancellationToken cancellationToken)
    {
        try
        {
            var producer = GetOrCreateProducer(topic);
            var partitionKey = ExtractPartitionKey(payload);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var message = new MessageMetadata();
            if (!string.IsNullOrEmpty(partitionKey))
            {
                message.Key = partitionKey;
            }
            await producer.Send(message, new ReadOnlySequence<byte>(payloadBytes), cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }

    private IProducer<ReadOnlySequence<byte>> GetOrCreateProducer(string topic)
    {
        if (_producers.TryGetValue(topic, out var existingProducer))
        {
            return existingProducer;
        }

        var producer = _pulsarClient.NewProducer()
            .Topic(topic)
            .Create();
        
        return producer;
    }

    private static string? ExtractPartitionKey(string payload)
    {
        using var doc = JsonDocument.Parse(payload);
        var root = doc.RootElement;
        if (root.TryGetProperty("OrderId", out var orderId)
            || root.TryGetProperty("orderId", out orderId))
        {
            return orderId.GetString();
        }
        return null;
    }
}