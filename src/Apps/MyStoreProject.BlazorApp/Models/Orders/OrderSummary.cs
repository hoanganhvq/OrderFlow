namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderSummary(Guid OrderId, string CustomerId, OrderStatus Status, decimal TotalPrice, DateTime CreatedAt) { }