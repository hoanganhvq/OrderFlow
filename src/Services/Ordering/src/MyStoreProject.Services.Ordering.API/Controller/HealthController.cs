using Microsoft.AspNetCore.Mvc;
using MyStoreProject.Services.Ordering.Infrastructure.Persistence;

namespace MyStoreProject.Services.Ordering.API.Controller;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly OrderDbContext _context;
    private readonly ILogger<HealthController> _logger;
    public HealthController(OrderDbContext context,  ILogger<HealthController> logger)
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