using BuildingBlocks.Common.Results;
using MediatR;
using MyStoreProject.Services.Payment.Application.DTOs;

namespace MyStoreProject.Services.Payment.Application.Payment.Queries;

public record GetByOrderIdQuery (Guid OrderId) : IRequest<Result<PaymentResponse>> { }