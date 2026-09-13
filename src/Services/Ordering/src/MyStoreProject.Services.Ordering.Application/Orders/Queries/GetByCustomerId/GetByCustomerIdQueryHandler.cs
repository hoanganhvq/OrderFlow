using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByCustomerId;

public class GetByCustomerIdQueryHandler  
    : IRequestHandler<GetByCustomerIdQuery, Result<IEnumerable< OrderResponse>>>
{
    
    private readonly IOrderDbContext _context;
   
    public GetByCustomerIdQueryHandler(IOrderDbContext context)
    {
        _context = context; 
    }
    
    public async Task<Result<IEnumerable< OrderResponse>>> Handle(GetByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders.AsNoTracking()
            .Where(o => o.CustomerId == request.CustomerId)
            .ToListAsync(cancellationToken);

        var response = orders.Select(order => new OrderResponse(order));
        return Result<IEnumerable< OrderResponse>>.Success(response);
    }
}