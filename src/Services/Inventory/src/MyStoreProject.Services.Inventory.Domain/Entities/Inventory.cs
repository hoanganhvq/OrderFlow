using BuildingBlocks.Common.Results;
using MyStoreProject.Services.Inventory.Domain.Errors;

namespace MyStoreProject.Services.Inventory.Domain.Entities;

public class Inventory 
{
    private Inventory() { }

    private Inventory(string sku, int initialQuantity)
    {
        Sku = sku;
        QuantityOnHand = initialQuantity;
        QuantityReserved = 0;
    }
    
    public string Sku { get; private set; }
    public int QuantityOnHand { get; private set; }
    public int QuantityReserved { get; private set; }
    
    public static Result<Inventory> Create(string sku, int initialQuantity)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return InventoryErrors.InvalidField("Sku");

        if (initialQuantity < 0)
            return InventoryErrors.InvalidField("Quantity");

        var stockItem = new Inventory(sku, initialQuantity);
        return Result<Inventory>.Success(stockItem);
    }
    
    public Result Restock(int quantity)
    {
        if (quantity <= 0)
        {
            return InventoryErrors.InvalidField("Quantity");
        }
        QuantityOnHand += quantity;
        return Result.Success();
    }

    public Result ReserverStock(int quantity)
    {
        if (quantity <= 0)
        {
            return InventoryErrors.InvalidField("Quantity");
        }
        QuantityReserved += quantity;
        return Result.Success();
    }

    public Result ReleaseStock(int quantity)
    {
        if (quantity <= 0 || quantity > QuantityReserved)
        {
            return InventoryErrors.InvalidField("Quantity");
        }
        QuantityReserved -= quantity;
        return Result.Success();
    }

    public Result ComsumeStock(int quantity)
    {
        if(quantity <= 0 || quantity > QuantityOnHand || quantity > QuantityReserved)
        {
            return InventoryErrors.InvalidField("Quantity");
        }
        QuantityOnHand -= quantity;
        QuantityReserved -= quantity;
        return Result.Success();
    }
    
    
    
}