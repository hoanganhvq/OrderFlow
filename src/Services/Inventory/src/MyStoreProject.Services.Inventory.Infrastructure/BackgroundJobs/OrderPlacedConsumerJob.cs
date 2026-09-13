using BuildingBlocks.Messaging.Inbox;
using DotPulsar.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Inventory.Application.Reservation.Commands;
using MyStoreProject.Services.Inventory.Infrastructure.Persistence;

namespace MyStoreProject.Services.Inventory.Infrastructure.BackgroundJobs;

public class OrderPlacedConsumerJob : BaseEventConsumer<InventoryDbContext, OrderPlaced>
{
    public OrderPlacedConsumerJob(ILogger<OrderPlacedConsumerJob> logger,
        IPulsarClient pulsarClient,
        IServiceProvider serviceProvider
        ) : base(serviceProvider, pulsarClient, logger, PulsarTopics.OrderPlaced,
            PulsarSubscriptions.InventoryOrderPlaced)
    {
    }

    protected override async Task<bool> HandleEventAsync(OrderPlaced @event, 
        IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var sender =  serviceProvider.GetRequiredService<ISender>();
        var reserveInventoryCommand = new ReserveInventoryCommand(
            @event.EventId,
            @event.OrderId,
            @event.Items);

        var resultCommand = await sender.Send(reserveInventoryCommand, cancellationToken);
        return resultCommand.IsSuccess;
    }
}