using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyStoreProject.Services.Payment.Application.DTOs;
using MyStoreProject.Services.Payment.Application.Payment.Queries;

namespace MyStoreProject.Services.Payment.API.Controllers;

[ApiController]
[Route("Payments")]
public class PaymentController : ControllerBase
{
    private readonly ISender _sender;
    public  PaymentController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<PaymentResponse>> GetByOrderIdAsync(
        [FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var command = new GetByOrderIdQuery(orderId);
        var resultPayment = await _sender.Send(command, cancellationToken);
        return Ok(resultPayment);
    }
}