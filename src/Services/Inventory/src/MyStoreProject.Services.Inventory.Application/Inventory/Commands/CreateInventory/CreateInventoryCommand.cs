using BuildingBlocks.Common.Results;
using MediatR;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands;

public record CreateInventoryCommand(string Sku, int quantity) : IRequest<Result>{ }

