using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyStoreProject.Services.Inventory.Application.DTOs;
using MyStoreProject.Services.Inventory.Application.Inventory.Commands.UpdateInventory;
using MyStoreProject.Services.Inventory.Application.Inventory.Queries.GetAll;

namespace MyStoreProject.Services.Inventory.API.Controllers;

[ApiController]
[Route("inventory")]
public class InventoryController : ControllerBase
{
    private readonly ISender _sender;

    public InventoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryResponse>>> GetStockAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllQuery();
        var resultInventory =  await _sender.Send(query);
        return Ok(resultInventory.Value);
    }

    [HttpPost("{sku:string}/adjust")]
    public async Task<ActionResult<InventoryResponse>> AdjustInventory(
        string sku, [FromBody] int quantity)
    {
        var updateInventoryCommand = new UpdateInventoryCommand(sku, quantity);
        var inventoryResponse = await _sender.Send(updateInventoryCommand);
        return Ok(inventoryResponse);
    }
    
    
}