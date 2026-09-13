using BuildingBlocks.Common.Results;

namespace MyStoreProject.Services.Payment.Domain.Errors;

public class PaymentErrors
{
    public static Error CannotChangeStatus 
        => Error.Validation("Payment.Validation",  "Cannot change status");
    
    public static Error InvalidAmount
        => Error.Validation("Payment.Validation", "Invalid amount");
    
}