using FluentValidation;

namespace MyStoreProject.Services.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator  : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer id is required");
        
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Items are empty");
        
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.Sku).NotEmpty().WithMessage("Sky is required.");
            items.RuleFor(i => i.Price).NotEmpty().WithMessage("Price is required.");
            items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        });
    }
    
}