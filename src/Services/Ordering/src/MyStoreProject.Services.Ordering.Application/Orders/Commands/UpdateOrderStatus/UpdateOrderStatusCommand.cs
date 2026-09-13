using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Ordering.Domain.Enums;

namespace MyStoreProject.Services.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Guid EventId, Guid OrderId, OrderStatus Status) : IRequest<Result>;