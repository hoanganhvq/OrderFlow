using Microsoft.AspNetCore.Mvc;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;
using MyStoreProject.Services.Inventory.Infrastructure.Persistence;

namespace MyStoreProject.Services.Inventory.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly InventoryDbContext _context;
    private readonly ILogger<HealthController> _logger;
    public HealthController(InventoryDbContext context,  ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> CheckHealthAsync(CancellationToken cancellationToken)
    {
        var isDbHealthy = false;
        try
        {
            isDbHealthy = await _context.Database.CanConnectAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Healthy DB check faild", ex.Message);
        }

        if (isDbHealthy)
        {
            return Ok();
        }
        else
        {
            return StatusCode(500);
        }
    }
}