using System.Text.Json;
using BuildingBlocks.Common.Results;
using BuildingBlocks.Messaging.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;

namespace MyStoreProject.Services.Inventory.Application.Reservation.Commands;

public class ReserveInventoryCommandHandler : IRequestHandler<ReserveInventoryCommand, Result>
{
    private readonly IInventoryDbContext _context;
    public ReserveInventoryCommandHandler(IInventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        var skus = request.Items.Select(i => i.Sku).Distinct().ToList();
        var stockItems = await _context.Inventories
            .Where(s => skus.Contains(s.Sku))
            .ToDictionaryAsync(s => s.Sku, cancellationToken);
        var hasEnoughStock = true;
        string? failureReason = null;
        foreach (var item in request.Items)
        {
            if (!stockItems.TryGetValue(item.Sku, out var stock) ||
                (stock.QuantityOnHand - stock.QuantityReserved) < item.Quantity)
            {
                hasEnoughStock = false;
                failureReason = $"SKU {item.Sku} not enough.";
                break;
            }
        }

        if (hasEnoughStock)
        {
            foreach (var item in request.Items)
            {
                var stock = stockItems[item.Sku];
                var reserveResult = stock.ReserverStock(item.Quantity);
                if (!reserveResult.IsSuccess)
                {
                    return Result.Failure(reserveResult.Error);
                }

                var createdReservation =
                    Domain.Entities.Reservation.Create(request.OrderId, item.Sku, item.Quantity);
                if (!createdReservation.IsSuccess)
                {
                    return Result.Failure(createdReservation.Error);
                }

                await _context.Reservations.AddAsync(createdReservation.Value, cancellationToken);
            }

            var succeededEvent = new ReservationSucceeded(
                Guid.NewGuid(),
                request.OrderId,
                request.Items);

            var outboxMessage = new OutboxMessage(
                id: Guid.NewGuid(),
                eventId: succeededEvent.EventId,
                topic: PulsarTopics.ReservationSucceeded,
                payload: JsonSerializer.Serialize(succeededEvent)
            );

            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }
        else
        {
            var failedEvent = new ReservationFailed(
                eventId: Guid.NewGuid(),
                orderId: request.OrderId,
                reason: failureReason);

            var outboxMessage = new OutboxMessage(
                id: Guid.NewGuid(),
                eventId: failedEvent.EventId,
                topic: PulsarTopics.ReservationFailed,
                payload: JsonSerializer.Serialize(failedEvent));

            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }

        var inbox = new InboxMessage(request.EventId);
        await _context.InboxMessages.AddAsync(inbox, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}