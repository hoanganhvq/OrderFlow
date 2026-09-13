using MyStoreProject.Services.Payment.Domain.Enums;

namespace MyStoreProject.Services.Payment.Application.DTOs;

public record PaymentResponse
{
    public PaymentResponse(Domain.Entities.Payment payment)
    {
        Id = payment.Id;
        OrderId = payment.OrderId;
        Amount = payment.Amount;
        Status = payment.Status;
    }
    public Guid Id { get; }
    public Guid OrderId { get; }
    public decimal Amount { get;  }
    public PaymentStatus Status { get; }
    
}