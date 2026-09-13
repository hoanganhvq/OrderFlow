namespace MyStoreProject.BlazorApp.Models.Inventory;

public record InventoryDTO (string Sku, int QuantityOnHand, int QuantityReserved, int Available) { }