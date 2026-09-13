using BuildingBlocks.Common.Results;
using MyStoreProject.Services.Payment.Domain.Enums;
using MyStoreProject.Services.Payment.Domain.Errors;

namespace MyStoreProject.Services.Payment.Domain.Entities;

public class Payment 
{
    private Payment(){}

    private Payment(Guid orderId, decimal amount)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Status = PaymentStatus.Succeeded;
        Amount = amount;
    }
    
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }

    public static Result<Payment> Create(Guid orderId, decimal amount)
    {
        if (amount <= 0)
        {
            return PaymentErrors.InvalidAmount;
        }

        var payment = new Payment(orderId, amount);
        return Result<Payment>.Success(payment);
    }
    public void MarkAsSucceeded()
    {
        Status = PaymentStatus.Succeeded;
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
    }
    
}