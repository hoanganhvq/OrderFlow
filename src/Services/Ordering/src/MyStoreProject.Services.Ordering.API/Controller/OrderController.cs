using BuildingBlocks.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyStoreProject.Services.Ordering.API.DTO;
using MyStoreProject.Services.Ordering.Application.Orders.Commands.CreateOrder;
using MyStoreProject.Services.Ordering.Application.Orders.Queries.DTOs;
using MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByCustomerId;
using MyStoreProject.Services.Ordering.Application.Orders.Queries.GetByOrderId;
using CreateOrderResponse = MyStoreProject.Services.Ordering.Application.Orders.Commands.CreateOrder.CreateOrderResponse;

namespace MyStoreProject.Services.Ordering.API.Controller;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> CreateOrderAsync(
        [FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(
            request.CustomerId, 
            request.Items.Select(item => new OrderItemDTO(item.Sku, item.Quantity, item.UnitPrice)).ToList());

        var result = await _sender.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return result.ToProblemResult();
        }
        
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetByOrderIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrdersByCustomer(
        [FromQuery] string customerId, 
        CancellationToken cancellationToken)
    {
        var query = new GetByCustomerIdQuery(customerId);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result.Value);
    }
}