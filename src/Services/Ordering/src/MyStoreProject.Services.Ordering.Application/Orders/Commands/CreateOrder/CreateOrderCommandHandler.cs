using System.Text.Json;
using BuildingBlocks.Common.Results;
using BuildingBlocks.Messaging.Entities;
using MediatR;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;
using MyStoreProject.Services.Ordering.Domain.Entities;

namespace MyStoreProject.Services.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
{
    private readonly IOrderDbContext _context;
    
    public CreateOrderCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var createOrderResult = Order.Create(request.CustomerId);
        if (!createOrderResult.IsSuccess)
        {
            return Result<CreateOrderResponse>.Failure(createOrderResult.Error);
        }
        
        var order = createOrderResult.Value;

        foreach (var item in request.Items)
        {
            var addItemResult = order.AddOrderItem(item.Sku, item.Price, item.Quantity);
            if (!addItemResult.IsSuccess)
            {
                return Result<CreateOrderResponse>.Failure(addItemResult.Error);
            }
        }
        
        var submitResult = order.SubmitOrder();
        
        if (!submitResult.IsSuccess)
        {
            return Result<CreateOrderResponse>.Failure(submitResult.Error);
        }
        
        var orderPlaced = new OrderPlaced(Guid.NewGuid(), 
            order.Id, 
            order.TotalPrice, 
            order.OrderItems.Select(order => new OrderItemDTO(
                Sku:order.Sku,
                Quantity:order.Quantity,
                Price:order.UnitPrice)).ToList(), 
            order.CustomerId);

        var outboxMessage = new OutboxMessage(
            id: Guid.NewGuid(),
            eventId: orderPlaced.EventId,
            topic: PulsarTopics.OrderPlaced,
            payload: JsonSerializer.Serialize(orderPlaced));

        var sagaState = new OrderSagaState(order.Id);
        
        await _context.Orders.AddAsync(order);
        await _context.OutboxMessages.AddAsync(outboxMessage);
        await _context.OrderSagaStates.AddAsync(sagaState);
        await _context.SaveChangesAsync(cancellationToken);
        
        var response = new CreateOrderResponse(order.Id,  order.Id, order.Status);
        return Result<CreateOrderResponse>.Success(response);
    }
}