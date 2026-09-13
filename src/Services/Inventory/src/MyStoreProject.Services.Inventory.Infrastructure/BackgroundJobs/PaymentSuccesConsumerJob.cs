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

public class PaymentSuccesConsumerJob : BaseEventConsumer<InventoryDbContext, PaymentSucceeded>
{
    private  readonly IServiceProvider _serviceProvider;
    public PaymentSuccesConsumerJob(
        IServiceProvider serviceProvider,
        IPulsarClient pulsarClient,
        ILogger<PaymentSuccesConsumerJob> logger
    ) : base(serviceProvider, pulsarClient, logger,
        PulsarTopics.PaymentFailed, 
        PulsarSubscriptions.InventoryPaymentFailed)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async  Task<bool> HandleEventAsync(PaymentSucceeded @event, InventoryDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        var command = new ChangeStatusReservationCommand(
            @event.EventId,
            @event.OrderId,
            ReservationStatus.Consumed);

        var resultCommand =  await sender.Send(command, cancellationToken);
        return resultCommand.IsSuccess;

    }
    
}