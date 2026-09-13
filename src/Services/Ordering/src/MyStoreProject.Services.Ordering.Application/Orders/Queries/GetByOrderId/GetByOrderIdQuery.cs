using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByCustomerId;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByOrderId;


public record GetByOrderIdQuery(Guid OrderId) : IRequest<Result<OrderResponse>>;

