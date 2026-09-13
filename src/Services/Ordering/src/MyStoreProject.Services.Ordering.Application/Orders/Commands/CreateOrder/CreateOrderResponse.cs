using MyStoreProject.Services.Ordering.Domain.Enums;

namespace MyStoreProject.Services.Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderResponse (Guid OrderId, Guid correlationId, OrderStatus Status) { }