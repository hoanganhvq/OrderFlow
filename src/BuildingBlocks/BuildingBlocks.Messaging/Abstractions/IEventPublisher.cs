using BuildingBlocks.Messaging.Entities;

namespace BuildingBlocks.Messaging.Abstractions;

public interface IEventPublisher
{
    public Task PublishAsync(string topic, string payload, CancellationToken cancellationToken);
}