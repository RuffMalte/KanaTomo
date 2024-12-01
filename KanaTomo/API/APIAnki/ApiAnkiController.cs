using KanaTomo.Models.Anki;

namespace KanaTomo.API.APIAnki;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class ApiAnkiController : ControllerBase
{
    private readonly IApiAnkiService _ankiService;

    public ApiAnkiController(IApiAnkiService ankiService)
    {
        _ankiService = ankiService;
    }

    [Authorize]
    [HttpPost("addCard")]
    public async Task<ActionResult<AnkiModel>> AddCardToUser([FromBody] AnkiModel ankiItem)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var userGuid = Guid.Parse(userId);
        var createdAnkiItem = await _ankiService.AddCardToUserAsync(userGuid, ankiItem);
        return CreatedAtAction(nameof(GetAnkiItem), new { id = createdAnkiItem.Id }, createdAnkiItem);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AnkiModel>> GetAnkiItem(Guid id)
    {
        var ankiItem = await _ankiService.GetAnkiItemByIdAsync(id);
        if (ankiItem == null)
        {
            return NotFound();
        }
        return Ok(ankiItem);
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<AnkiModel>>> GetUserAnkiItems()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var userGuid = Guid.Parse(userId);
        var ankiItems = await _ankiService.GetAnkiItemsByUserIdAsync(userGuid);
        return Ok(ankiItems);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnkiItem(Guid id, [FromBody] AnkiModel ankiItem)
    {
        if (id != ankiItem.Id)
        {
            return BadRequest();
        }

        var updatedItem = await _ankiService.UpdateAnkiItemAsync(ankiItem);
        if (updatedItem == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnkiItem(Guid id)
    {
        var result = await _ankiService.DeleteAnkiItemAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}