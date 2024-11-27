using KanaTomo.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.API;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class DatabaseController : ControllerBase
{
    private readonly UserContext _context;

    public DatabaseController(UserContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            await _context.Database.CanConnectAsync();
            return Ok("Database connection successful");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Database connection failed: {ex.Message}");
        }
    }
}