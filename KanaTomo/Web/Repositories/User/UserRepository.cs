using System.Net.Http.Headers;
using KanaTomo.Models.User;

namespace KanaTomo.Web.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _baseUrl;

    public UserRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _baseUrl = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) 
            ? "http://localhost:5070" 
            : "http://host.docker.internal:5070";
    }

    public async Task<UserModel> GetCurrentUserAsync()
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
}