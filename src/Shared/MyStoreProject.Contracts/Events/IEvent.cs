namespace MyStoreProject.Contracts.Events;

public interface IEvent
{
    public Guid EventId { get; }
    public Guid OrderId { get; }
    public DateTime Timestamp { get; } 
}