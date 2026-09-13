namespace MyStoreProject.Services.Inventory.Application.DTOs;

public record InventoryResponse
{
    public InventoryResponse(Domain.Entities.Inventory inventory)
    {
        Sku = inventory.Sku;
        QuantityOnHand =  inventory.QuantityOnHand;
        QuantityReserved = inventory.QuantityReserved;
    }
    
    public string Sku { get; }
    public int QuantityOnHand { get; }
    public int QuantityReserved { get; }
}