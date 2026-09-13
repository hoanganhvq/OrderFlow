
namespace MyStoreProject.Contracts.Events;

public record PaymentSucceeded  : IEvent
{

    public PaymentSucceeded(Guid eventId, Guid orderId, decimal amount, Guid paymentId)
    {
        EventId = eventId;
        OrderId = orderId;
        Timestamp =  DateTime.UtcNow;
        Amount = amount;
        PaymentId = paymentId;
    }
    
    public Guid EventId { get; }
    public Guid OrderId { get; }
    public DateTime Timestamp { get; }
    public decimal Amount { get; }
    public Guid PaymentId { get; }
}