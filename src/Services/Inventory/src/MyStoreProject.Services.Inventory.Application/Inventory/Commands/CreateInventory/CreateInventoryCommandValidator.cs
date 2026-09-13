using FluentValidation;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands;

public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(createdInventory => createdInventory.Sku)
            .NotEmpty()
            .WithMessage("Sku cannot be empty");
        
        RuleFor(createdInventory => createdInventory.quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");
        
    }
}