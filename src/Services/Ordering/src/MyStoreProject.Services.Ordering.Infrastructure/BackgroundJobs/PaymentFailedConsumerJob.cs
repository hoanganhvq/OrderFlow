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

public class PaymentFailedConsumerJob : BaseEventConsumer<OrderDbContext, PaymentFailed>
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentFailedConsumerJob(
        IServiceProvider serviceProvider,
        IPulsarClient pulsarClient,
        ILogger<PaymentFailedConsumerJob> logger) 
        : base(serviceProvider, pulsarClient, logger, PulsarTopics.PaymentFailed, PulsarSubscriptions.OrdersPaymentFailed)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<bool> HandleEventAsync(PaymentFailed @event, OrderDbContext dbContext, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        
        var command = new UpdateOrderStatusCommand(
            @event.EventId,
            @event.OrderId,
            OrderStatus.Cancelled);
                            
        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess;
    }
}