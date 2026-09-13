using MyStoreProject.Contracts.Common;

namespace MyStoreProject.Contracts.Events;

public record OrderPlaced : IEvent
{
    public OrderPlaced(Guid eventId, Guid  orderId, decimal totalPrice, List<OrderItemDTO> items, string customerId)
    {
        EventId = eventId;
        OrderId = orderId;
        TotalPrice = totalPrice;
        Items = items;
        CustomerId = customerId;    
    }
    public Guid EventId { get; set; }
    public Guid OrderId { get; set; }
    public DateTime Timestamp { get; set; } =  DateTime.UtcNow;
    public List<OrderItemDTO> Items { get; set; }
    public string CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
} 