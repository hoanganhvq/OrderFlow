using BuildingBlocks.Common.Results;

namespace MyStoreProject.Services.Inventory.Domain.Errors;

public class InventoryErrors
{
        public static Error InvalidField(string fieldName) 
                => Error.Validation("Inventory.Validation", $"Invalid {fieldName}");
        public static Error InventoryNotFound(string sku)
                => Error.NotFound("Inventory.NotFound", $"Inventory with {sku} Not Found");
        
        public static Error ConcurrencyError
                => Error.Conflict("Inventory.Conflict", "Concurrency Error");
        
}