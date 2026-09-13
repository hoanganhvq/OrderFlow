using MyStoreProject.Contracts.Common;

namespace MyStoreProject.Contracts.Events;

public record ReservationSucceeded : IEvent
{
    public ReservationSucceeded(Guid eventId, 
        Guid orderId,
        List<OrderItemDTO> items)
    {
        EventId = eventId;
        OrderId = orderId;
        Timestamp =  DateTime.UtcNow;
        Items =  items;
    }
    
    public Guid EventId { get; }
    public Guid OrderId { get; }
    public DateTime Timestamp { get; }
    public List<OrderItemDTO> Items { get; }
}