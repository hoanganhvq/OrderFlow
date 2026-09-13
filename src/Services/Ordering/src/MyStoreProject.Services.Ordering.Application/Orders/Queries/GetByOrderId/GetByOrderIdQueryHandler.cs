using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;
using MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByCustomerId;
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
            .SingleOrDefaultAsync(o => o.Id == request.OrderId);
        
        if (order is null)
        {
            return Result<OrderResponse>.Failure(OrderErrors.OrderNotFound);   
        }

        var response = new OrderResponse(order);
        return Result<OrderResponse>.Success(response);
    }
}