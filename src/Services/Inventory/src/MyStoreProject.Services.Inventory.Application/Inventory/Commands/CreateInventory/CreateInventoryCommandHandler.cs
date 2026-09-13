using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands;

public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, Result>
{
    private readonly IInventoryDbContext _context;
    
    public CreateInventoryCommandHandler(IInventoryDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        var createdInventory = Domain.Entities.Inventory.Create(request.Sku, request.quantity);

        if (!createdInventory.IsSuccess)
        {
            return Result.Failure(createdInventory.Error);
        }

        await _context.Inventories.AddAsync(createdInventory.Value);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}