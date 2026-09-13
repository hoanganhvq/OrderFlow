using MyStoreProject.Services.Ordering.Domain.Enums;

namespace MyStoreProject.Services.Ordering.API.DTO;

public record CreateOrderResponse
{
    public Guid OrderId { get; init; }
    public Guid CorrelationId { get; init; }
    public OrderStatus Status { get; init; }
}