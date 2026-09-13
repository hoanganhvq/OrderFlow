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

public class ReservationFailedConsumerJob : BaseEventConsumer<OrderDbContext, ReservationFailed>
{
    private readonly IServiceProvider _serviceProvider;

    public ReservationFailedConsumerJob(
        IServiceProvider serviceProvider,
        IPulsarClient pulsarClient,
        ILogger<ReservationFailedConsumerJob> logger)
    : base(serviceProvider, pulsarClient, logger, PulsarTopics.ReservationFailed, PulsarSubscriptions.OrdersReservationFailed)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<bool> HandleEventAsync(ReservationFailed @event, OrderDbContext dbContext,
        CancellationToken cancellationToken = default)
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