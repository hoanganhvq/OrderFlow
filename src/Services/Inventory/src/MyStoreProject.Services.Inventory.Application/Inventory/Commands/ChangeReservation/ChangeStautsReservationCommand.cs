using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Inventory.Domain.Enum;

namespace MyStoreProject.Services.Inventory.Application.Inventory.Commands.ReleaseReservation;

public record ChangeStatusReservationCommand (Guid EventId, Guid OrderId, ReservationStatus Status) : IRequest<Result> { }