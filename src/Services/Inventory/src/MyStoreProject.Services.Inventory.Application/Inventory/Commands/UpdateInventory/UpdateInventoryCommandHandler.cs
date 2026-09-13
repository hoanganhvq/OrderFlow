using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;
using MyStoreProject.Services.Inventory.Application.DTOs;
using MyStoreProject.Services.Inventory.Domain.Errors;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands.UpdateInventory;

public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, Result<InventoryResponse>>
{
    private readonly IInventoryDbContext _context;
    public UpdateInventoryCommandHandler(IInventoryDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<InventoryResponse>> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {

        var inventoryExist = await _context.Inventories
            .Where(i => i.Sku == request.Sku)
            .FirstOrDefaultAsync(cancellationToken);

        if (inventoryExist is null)
        {
            return Result<InventoryResponse>.Failure(InventoryErrors.InventoryNotFound(request.Sku));
        }

        var resultRestockInventory = inventoryExist.Restock(request.Quantity);
        if (!resultRestockInventory.IsSuccess)
        {
            return Result<InventoryResponse>.Failure(resultRestockInventory.Error);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        return Result<InventoryResponse>.Success(new InventoryResponse(inventoryExist));
    }
}