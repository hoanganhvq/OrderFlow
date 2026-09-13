namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderResponse (Guid OrderId, Guid CorrelationId, OrderStatus Status){ }