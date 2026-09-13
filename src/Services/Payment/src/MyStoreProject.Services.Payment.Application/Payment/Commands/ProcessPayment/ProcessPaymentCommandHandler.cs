using System.Text.Json;
using BuildingBlocks.Common.Results;
using BuildingBlocks.Messaging.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Payment.Application.Abstractions.Data;

namespace MyStoreProject.Services.Payment.Application.Payment.Commands.ProcessPayment;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result>
{
    private readonly IPaymentDbContext _context;
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;
    
    public ProcessPaymentCommandHandler(
        IPaymentDbContext paymentDbContext,
        ILogger<ProcessPaymentCommandHandler> logger)
    {
        _context = paymentDbContext;
        _logger = logger;
    }
    
    
    public async Task<Result> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentCreatedResult = Domain.Entities.Payment.Create(request.OrderId, request.Amount);
        if (!paymentCreatedResult.IsSuccess)
        {
            return Result.Failure(paymentCreatedResult.Error);
        }

        var payment = paymentCreatedResult.Value;
        var isSuccess = Random.Shared.Next(1, 4) != 1; 
        OutboxMessage outboxMessage;
        
        if (isSuccess) 
        {
            payment.MarkAsSucceeded();
            var paymentSucceeded = new PaymentSucceeded(
                eventId:Guid.NewGuid(),
                orderId:payment.OrderId,
                paymentId:payment.Id,
                amount:payment.Amount);

            outboxMessage = new OutboxMessage(
                id: Guid.NewGuid(),
                eventId: paymentSucceeded.EventId,
                topic: PulsarTopics.PaymentSucceeded,
                payload: JsonSerializer.Serialize(paymentSucceeded));
        }
        else
        {
            payment.MarkAsFailed();
            var paymentFailed = new PaymentFailed(
                eventId: Guid.NewGuid(),
                orderId: payment.OrderId,
                reason: "Not Enough Money");
            
            outboxMessage = new OutboxMessage(
                id: Guid.NewGuid(),
                eventId: Guid.NewGuid(),
                topic: PulsarTopics.PaymentFailed,
                payload: JsonSerializer.Serialize(paymentFailed));
        }
        
        await _context.Payments.AddAsync(payment);
        await _context.OutboxMessages.AddAsync(outboxMessage);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}