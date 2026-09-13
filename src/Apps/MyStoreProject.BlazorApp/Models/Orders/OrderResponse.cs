namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderResponse (Guid OrderId, Guid CorrelationId, string Status){ }