using BuildingBlocks.Common.Results;

namespace MyStoreProject.Services.Inventory.Domain.Errors;

public class ReservationErrors
{
     public static Error CannotChangeStatus 
         => Error.Validation("Reservation.Validation", "Cannot change status");
     
     public static Error InvalidField(string field)
         => Error.Validation("Reservation.Validation", $"Invalid {field}");
     
     
     
}