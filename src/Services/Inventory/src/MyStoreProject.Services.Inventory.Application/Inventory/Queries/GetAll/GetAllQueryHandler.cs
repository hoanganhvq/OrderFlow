using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;
using MyStoreProject.Services.Inventory.Application.DTOs;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Queries.GetAll;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Result<IEnumerable<InventoryResponse>>>
{
    private readonly IInventoryDbContext _context;
    
    public GetAllQueryHandler(IInventoryDbContext context)
    {
        _context = context; 
    }
    
    public async Task<Result<IEnumerable<InventoryResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var inventory = await _context.Inventories
            .ToListAsync(cancellationToken);

        var inventoryResponse = inventory.Select(i => new InventoryResponse(i));
        
        return Result<IEnumerable<InventoryResponse>>.Success(inventoryResponse);
    }
}