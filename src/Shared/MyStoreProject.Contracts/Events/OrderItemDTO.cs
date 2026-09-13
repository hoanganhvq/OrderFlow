namespace MyStoreProject.Contracts.Events;

public record OrderItemDTO (string Sku, int Quantity, decimal Price) { }