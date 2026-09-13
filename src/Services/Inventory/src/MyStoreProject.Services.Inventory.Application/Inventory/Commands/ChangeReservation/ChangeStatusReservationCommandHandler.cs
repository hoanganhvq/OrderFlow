using BuildingBlocks.Common.Results;
using BuildingBlocks.Messaging.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;
using MyStoreProject.Services.Inventory.Domain.Enum;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands.ReleaseReservation;

public class ChangeStatusReservationCommandHandler : IRequestHandler<ChangeStatusReservationCommand, Result>
{
    private readonly IInventoryDbContext _context;

    public ChangeStatusReservationCommandHandler(IInventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ChangeStatusReservationCommand request, CancellationToken cancellationToken)
    {
        var reservations = await _context.Reservations
            .Where(r => r.OrderId == request.OrderId && r.Status == ReservationStatus.Active) 
            .ToListAsync(cancellationToken);

        if (!reservations.Any())
        {
            return Result.Success();
        }

        var skus = reservations.Select(r => r.Sku).Distinct().ToList();
        var stockItems = await _context.Inventories
            .Where(s => skus.Contains(s.Sku))
            .ToDictionaryAsync(s => s.Sku, cancellationToken);

        foreach (var reservation in reservations)
        {
            if (!stockItems.TryGetValue(reservation.Sku, out var stock))
            {
                return Result.Failure(new Error("Inventory.NotFound", $"SKU {reservation.Sku} not found in inventory.", ErrorType.NotFound));
            }

            if (request.Status == ReservationStatus.Released)
            {
                var releaseResult = stock.ReleaseStock(reservation.Quantity);
                if (!releaseResult.IsSuccess)
                {
                    return Result.Failure(releaseResult.Error);
                }
                reservation.Release();
            }
            else if (request.Status == ReservationStatus.Consumed)
            {
                var consumeResult = stock.ComsumeStock(reservation.Quantity);
                if (!consumeResult.IsSuccess)
                {
                    return Result.Failure(consumeResult.Error);
                }
                reservation.Consume();
            }
        }

        var inboxMessage = new InboxMessage(request.EventId);
        await _context.InboxMessages.AddAsync(inboxMessage, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}