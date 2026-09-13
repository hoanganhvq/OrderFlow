using MyStoreProject.Services.Ordering.Domain.Entities;
using MyStoreProject.Services.Ordering.Domain.Enums;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries;
public record OrderResponse
{
    public OrderResponse(Order order)
    {
        OrderId = order.Id;
        CustomerId = order.CustomerId;
        Status = order.Status;
        TotalPrice =  order.TotalPrice;
        //ReservationCompleted;
        //PaymentCompelted;
        CreatedAt = order.CreatedAt;
        UpdatedAt = order.UpdatedAt;
    }
    
    public Guid OrderId { get; init; }
    public string CustomerId { get; init; }
    public OrderStatus Status { get; init; }
    public decimal TotalPrice { get; init; }
    public bool ReservationCompleted { get; init; }
    public bool PaymentCompleted { get; init; }
    public List<OrderItem> OrderItems { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}