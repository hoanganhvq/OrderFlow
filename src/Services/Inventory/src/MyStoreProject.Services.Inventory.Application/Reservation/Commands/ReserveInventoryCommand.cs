using BuildingBlocks.Common.Results;
using MediatR;

namespace MyStoreProject.Services.Inventory.Application.Reservation.Commands;

public record ReserveInventoryCommand(Guid EventId, Guid OrderId, List<Contracts.Events.OrderItemDTO> Items) : IRequest<Result> {}