using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Ordering.Application.Orders.Queries.DTOs;

namespace MyStoreProject.Services.Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string CustomerId, List<OrderItemDTO> Items) : IRequest<Result<CreateOrderResponse>>;
