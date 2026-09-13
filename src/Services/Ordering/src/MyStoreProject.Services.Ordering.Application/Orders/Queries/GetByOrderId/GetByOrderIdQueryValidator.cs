using FluentValidation;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByOrderId;

public class GetByOrderIdQueryValidator : AbstractValidator<GetByOrderIdQuery>
{
    public GetByOrderIdQueryValidator()
    {
        RuleFor(o => o.OrderId) 
            .NotEmpty()
            .WithMessage("OrderId cannot be empty");
    }
}