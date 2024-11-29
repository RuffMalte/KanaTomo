using System.Net.Http.Headers;
using KanaTomo.Models.User;
using KanaTomo.Web.Services.User;

namespace KanaTomo.Web.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string? _baseUrl;
    private readonly IConfiguration _configuration;

    public UserRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _baseUrl = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) 
            ? _configuration.GetValue<string>("Connections:localhost") 
            : _configuration.GetValue<string>("Connections:docker");
    }

    public async Task<UserModel?> GetCurrentUserAsync()
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/apiusers/me");
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserModel>();
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user)
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("No authentication token found");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/v1/apiusers/{user.Id}", user);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to update user");
        }
        
        var result = await response.Content.ReadFromJsonAsync<UserModel>();
        
        if (result == null)
        {
            throw new Exception("Failed to update user");
        }

        return result;
    }
    
    public async Task DeleteUserAsync(Guid id)
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("No authentication token found");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/apiusers/{id}");
    
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to delete user");
        }
    
        response.EnsureSuccessStatusCode();

        // Delete the auth token from cookies
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("AuthToken");
    }
}