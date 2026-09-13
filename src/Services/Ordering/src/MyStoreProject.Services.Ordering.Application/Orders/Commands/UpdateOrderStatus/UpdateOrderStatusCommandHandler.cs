using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;
using MyStoreProject.Services.Ordering.Domain.Entities;
using MyStoreProject.Services.Ordering.Domain.Enums;
using MyStoreProject.Services.Ordering.Domain.Errors;

namespace MyStoreProject.Services.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IOrderDbContext _context;

    public UpdateOrderStatusCommandHandler(IOrderDbContext context) => _context = context;

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure(OrderErrors.OrderNotFound);
        }

        var result = request.Status switch
        {
            OrderStatus.Reserving => order.MarkAsReserving(),
            OrderStatus.Charging  => order.MarkAsCharging(),
            OrderStatus.Confirmed => order.MarkAsConfirmed(),
            OrderStatus.Cancelled => order.Cancel(),
            _ => OrderErrors.CannotChangStatus
        };
        if (!result.IsSuccess)
        {
            return result;
        }
        var sagaState = await _context.OrderSagaStates
            .FindAsync(request.OrderId, cancellationToken);

        if (sagaState is null)
        {
            sagaState = new OrderSagaState(request.OrderId);
            await _context.OrderSagaStates.AddAsync(sagaState, cancellationToken);
        }

        switch (request.Status)
        {
            case OrderStatus.Charging:
                sagaState.MarkReservationCompleted(request.EventId);
                break;

            case OrderStatus.Confirmed:
                sagaState.MarkPaymentCompleted(request.EventId);
                break;

            default:
                sagaState.RecordEvent(request.EventId);
                break;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}