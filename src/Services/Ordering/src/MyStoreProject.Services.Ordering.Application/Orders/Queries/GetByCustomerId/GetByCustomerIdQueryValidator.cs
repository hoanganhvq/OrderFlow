using FluentValidation;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByCustomerId;

public class GetByCustomerIdQueryValidator : AbstractValidator<GetByCustomerIdQuery>
{   
    public GetByCustomerIdQueryValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEmpty()
            .WithMessage("Customer Id cannot be empty");
    }
}