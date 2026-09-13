using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Inventory.Application.DTOs;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Queries.GetAll;

public record GetAllQuery : IRequest<Result<IEnumerable<InventoryResponse>>> { }