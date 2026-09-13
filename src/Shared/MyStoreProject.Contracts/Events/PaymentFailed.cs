namespace MyStoreProject.Contracts.Events;

public record PaymentFailed : IEvent
{
    public PaymentFailed(Guid eventId, Guid orderId, string reason)
    {
        EventId = eventId;
        OrderId = orderId;
        Timestamp =  DateTime.UtcNow;
        Reason = reason;
    }
    public Guid EventId { get; }
    public Guid OrderId { get; }
    public DateTime Timestamp { get; }
    public string Reason { get; }
}