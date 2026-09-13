using MyStoreProject.Services.Ordering.Application.Orders.Queries.DTOs;
using MyStoreProject.Services.Ordering.Domain.Entities;
using MyStoreProject.Services.Ordering.Domain.Enums;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries;
public record OrderResponse
{
    public OrderResponse(Order order, OrderSagaState? sagaState)
    {
        OrderId = order.Id;
        CustomerId = order.CustomerId;
        Status = order.Status;
        TotalPrice =  order.TotalPrice;
        ReservationCompleted = sagaState?.ReservationCompleted ?? false;
        PaymentCompleted = sagaState?.PaymentCompleted ?? false;
        Items = order.OrderItems.Select(i => new OrderItemDTO(i.Sku, i.Quantity, i.UnitPrice)).ToList();
        CreatedAt = order.CreatedAt;
        UpdatedAt = order.UpdatedAt;
    }
    
    public Guid OrderId { get; init; }
    public string CustomerId { get; init; }
    public OrderStatus Status { get; init; }
    public decimal TotalPrice { get; init; }
    public bool ReservationCompleted { get; init; }
    public bool PaymentCompleted { get; init; }
    public List<OrderItemDTO> Items { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}