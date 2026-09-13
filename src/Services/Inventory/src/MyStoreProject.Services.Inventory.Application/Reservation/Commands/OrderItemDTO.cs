namespace MyStoreProject.Services.Inventory.Application.Reservation.Commands;

public record OrderItemDTO (string Sku, int Quantity, decimal Price) { }