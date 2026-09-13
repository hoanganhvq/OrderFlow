using FluentValidation;

namespace MyStoreProject.Services.Payment.Application.Payment.Queries;

public class GetByOrderIdQueryValidator : AbstractValidator<GetByOrderIdQuery>
{
    public GetByOrderIdQueryValidator()
    {
        RuleFor(p => p.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be empty");
    }
}