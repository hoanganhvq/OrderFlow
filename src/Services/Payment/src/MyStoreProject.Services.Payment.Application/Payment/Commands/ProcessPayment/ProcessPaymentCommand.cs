using BuildingBlocks.Common.Results;
using MediatR;

namespace MyStoreProject.Services.Payment.Application.Payment.Commands.ProcessPayment;

public record ProcessPaymentCommand (Guid EventId, Guid OrderId, decimal Amount)  :IRequest<Result> { }