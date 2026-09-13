namespace MyStoreProject.BlazorApp.Models.Orders;

public record OrderItemDTO(string Sku, int Quantity, decimal UnitPrice) { }