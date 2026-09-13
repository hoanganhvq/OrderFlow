using FluentValidation;

namespace MyStoreProject.Services.Inventory.Application.Reservation.Commands;

public class ReserveInventoryCommandValidator  : AbstractValidator<ReserveInventoryCommand>
{
    public ReserveInventoryCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be empty");;
        
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("EventId cannot be empty");
    }
}