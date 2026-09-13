using BuildingBlocks.Common.Results;

namespace MyStoreProject.Services.Ordering.Domain.Errors;

public static class OrderErrors
{
    public static Error CannotAddItems 
        => Error.Validation("Order.Validation", "Can not add items to this order");
    
    public static Error CannotRemoveItems 
        => Error.Validation("Order.Validation", "Can not remove items from this order.");

    public static Error EmptyItems 
        => Error.Validation("Order.Validation", "Can not add items to this order");
    
    public static  Error ItemNotFound 
        => Error.NotFound("Order.NotFound", "Item not found");
    
    public static Error OrderNotFound 
        => Error.NotFound("Order.NotFound", "Order not found");    
    
    public static  Error AlreadyCompleted 
        => Error.Validation("Order.Validation", "Order already completed");
    
    public static  Error CannotChangStatus 
        => Error.Validation("Order.Validation", "Cannot change status");

    public static  Error InvalidQuantity 
        => Error.Validation("Order.Validation", "Invalid quantity");
    
    public static Error InvalidField(string field) 
        => Error.Validation("Order.Validation", $"Invalid {field} ");
}