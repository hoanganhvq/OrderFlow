using FluentValidation;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands.ReleaseReservation;

public class ChangeStatusReservationCommandValidator : AbstractValidator<ChangeStatusReservationCommand>
{
    public ChangeStatusReservationCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be empty");
        
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("EventId cannot be empty");
    }
}