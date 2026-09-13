using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyStoreProject.Services.Payment.Application.Abstractions.Data;
using MyStoreProject.Services.Payment.Application.DTOs;

namespace MyStoreProject.Services.Payment.Application.Payment.Queries;

public class GetByOrderIdByQueryHandler : IRequestHandler<GetByOrderIdQuery, Result<PaymentResponse>>
{
    private readonly IPaymentDbContext _context;
    public GetByOrderIdByQueryHandler(IPaymentDbContext context)
    {
        _context = context;
    }
    public async Task<Result<PaymentResponse>> Handle(GetByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .Where(p => p.OrderId == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);

        var paymentResponse = new PaymentResponse(payment);
        return Result<PaymentResponse>.Success(paymentResponse);

    }
}