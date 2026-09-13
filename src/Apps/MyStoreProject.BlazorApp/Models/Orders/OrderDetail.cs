namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderDetail
{
    public Guid OrderId { get; init; }
    public string CustomerId { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public decimal TotalPrice { get; init; }
    public bool ReservationCompleted { get; init; }
    public bool PaymentCompleted { get; init; }
    public List<OrderItemDTO> Items { get; init; } = new();
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}