using KanaTomo.API.APIAuth;
using KanaTomo.Models.Auth;
using KanaTomo.Models.User;

namespace KanaTomo.Web.Controllers.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly HttpClient _httpClient;
    private readonly string? _baseUrl;

    public AuthRepository(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) 
            ? configuration.GetValue<string>("Connections:localhost") 
            : configuration.GetValue<string>("Connections:docker");
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var loginDto = new LoginDto { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/apiauth/login", loginDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result.Token;
    }

    public async Task<string> RegisterAsync(string username, string password, string email)
    {
        var registerDto = new RegisterDto { Username = username, Password = password, Email = email };
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/apiauth/register", registerDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result.Token;
    }
}