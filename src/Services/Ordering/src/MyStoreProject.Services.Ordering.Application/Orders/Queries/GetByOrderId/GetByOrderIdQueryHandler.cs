using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;
using MyStoreProject.Services.Ordering.Domain.Errors;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByOrderId;

public class GetOrderByIdQueryHandler : IRequestHandler<GetByOrderIdQuery, Result<OrderResponse>>
{
    
    private readonly IOrderDbContext _context;

    public GetOrderByIdQueryHandler(IOrderDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<OrderResponse>> Handle(GetByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .SingleOrDefaultAsync(o => o.Id == request.OrderId);
        
        if (order is null)
        {
            return Result<OrderResponse>.Failure(OrderErrors.OrderNotFound);   
        }
        
        var saga = await _context.OrderSagaStates.AsNoTracking()
            .Where(s => s.OrderId == order.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        var response = new OrderResponse(order, saga);
        return Result<OrderResponse>.Success(response);
    }
}