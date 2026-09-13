using BuildingBlocks.Messaging.Inbox;
using DotPulsar.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Inventory.Application.Inventory.Commands.ReleaseReservation;
using MyStoreProject.Services.Inventory.Domain.Enum;
using MyStoreProject.Services.Inventory.Infrastructure.Persistence;

namespace MyStoreProject.Services.Inventory.Infrastructure.BackgroundJobs;

public class PaymentFailedConsumerJob : BaseEventConsumer<InventoryDbContext, PaymentFailed>
{
    public PaymentFailedConsumerJob(
        IServiceProvider serviceProvider,
        IPulsarClient pulsarClient,
        ILogger<PaymentFailedConsumerJob> logger
        ) : base(serviceProvider, pulsarClient, logger,
        PulsarTopics.PaymentFailed, 
        PulsarSubscriptions.InventoryPaymentFailed)
    {
    }

    protected override async Task<bool> HandleEventAsync(PaymentFailed @event, 
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var sender = serviceProvider.GetRequiredService<ISender>();

        var command = new ChangeStatusReservationCommand(
            @event.EventId,
            @event.OrderId,
            ReservationStatus.Released);

        var resultCommand = await sender.Send(command, cancellationToken);
        
        return resultCommand.IsSuccess;
    }
}