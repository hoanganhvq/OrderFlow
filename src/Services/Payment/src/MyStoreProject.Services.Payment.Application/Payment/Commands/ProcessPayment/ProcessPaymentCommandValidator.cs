using FluentValidation;

namespace MyStoreProject.Services.Payment.Application.Payment.Commands.ProcessPayment;

public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(p => p.EventId)
            .NotEmpty()
            .WithMessage("EventId cannot be empty");
        
        RuleFor(p=>p.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be empty");
        
        RuleFor(p => p.Amount)
            .NotEmpty()
            .WithMessage("Amount cannot be empty");
    }
}