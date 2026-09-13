namespace MyStoreProject.Services.Inventory.Application.DTOs;

public class InventoryRequest 
{
    public InventoryRequest(string sku, int quantityOnHand)
    {
        Sku = sku;
        QuantityOnHand = quantityOnHand;
    }
    public string Sku { get;}
    public int QuantityOnHand { get; }
}