using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Inventory.Application.DTOs;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands.UpdateInventory;

public record UpdateInventoryCommand(string Sku, int Quantity) : IRequest<Result<InventoryResponse>>
{
}