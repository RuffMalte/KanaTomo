using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KanaTomo.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace KanaTomo.API.APIAuth;

public interface IApiAuthService
{
    Task<string> LoginAsync(string username, string password);
    Task<string> RegisterAsync(string username, string password);
}

public class ApiAuthService : IApiAuthService
{
    private readonly IApiAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public ApiAuthService(IApiAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _authRepository.GetUserByUsernameAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        return GenerateJwtToken(user);
    }

    public async Task<string> RegisterAsync(string username, string password)
    {
        var existingUser = await _authRepository.GetUserByUsernameAsync(username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists");
        }

        var newUser = new UserModel
        {
            Username = username,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _authRepository.CreateUserAsync(newUser, password);
        return GenerateJwtToken(createdUser);
    }

    private string GenerateJwtToken(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("jwtSecret"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}