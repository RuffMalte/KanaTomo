using KanaTomo.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.API.APIAuth;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class ApiAuthController : ControllerBase
{
    private readonly IApiAuthService _authService;

    public ApiAuthController(IApiAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var token = await _authService.RegisterAsync(registerDto.Username, registerDto.Password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RegisterDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}