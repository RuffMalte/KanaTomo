using System.Security.Claims;
using KanaTomo.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanaTomo.API.APIUser;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class ApiUsersController : ControllerBase
{
    private readonly IApiUserService _userService;

    public ApiUsersController(IApiUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserModel>> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserModel>> CreateUser(UserModel user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserModel user)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || id != Guid.Parse(userId))
        {
            return Unauthorized();
        }

        if (id != user.Id)
        {
            user.Id = id; // Ensure the ID matches
        }

        try
        {
            await _userService.UpdateUserAsync(user);
            return Ok(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    private async Task<bool> UserExists(Guid id)
    {
        return await _userService.GetUserByIdAsync(id) != null;
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserModel>> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}