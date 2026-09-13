using BuildingBlocks.Common.Results;
using MediatR;

namespace MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByCustomerId;

public record GetByCustomerIdQuery(string CustomerId)  : IRequest<Result<IEnumerable< OrderResponse>>> { }