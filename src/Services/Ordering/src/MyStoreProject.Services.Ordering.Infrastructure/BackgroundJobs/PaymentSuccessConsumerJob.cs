using BuildingBlocks.Messaging.Inbox;
using DotPulsar.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Ordering.Application.Orders.Commands.UpdateOrderStatus;
using MyStoreProject.Services.Ordering.Domain.Enums;
using MyStoreProject.Services.Ordering.Infrastructure.Persistence;

namespace MyStoreProject.Services.Ordering.Infrastructure.BackgroundJobs;

public class PaymentSuccessConsumerJob : BaseEventConsumer<OrderDbContext,PaymentSucceeded>
{

    public PaymentSuccessConsumerJob(IServiceProvider serviceProvider,
        ILogger<PaymentSuccessConsumerJob> logger,
        IPulsarClient pulsarClient)
    : base(serviceProvider, pulsarClient, logger, PulsarTopics.PaymentSucceeded, PulsarSubscriptions.OrdersPaymentSucceeded)
    {
    }

    protected override async Task<bool> HandleEventAsync(PaymentSucceeded @event, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var sender = serviceProvider.GetRequiredService<ISender>();

        var command = new UpdateOrderStatusCommand(
            @event.EventId,
            @event.OrderId,
            OrderStatus.Confirmed);
                        
        var result = await sender.Send(command,  cancellationToken);
        
        return result.IsSuccess;
    }
}