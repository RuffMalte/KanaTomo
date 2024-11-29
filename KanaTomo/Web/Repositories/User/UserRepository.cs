using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KanaTomo.Models.User;
using KanaTomo.Web.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace KanaTomo.Web.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _baseUrl;
    private const string AuthTokenCookieName = "AuthToken";

    public UserRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _baseUrl = GetBaseUrl(configuration);
    }

    public async Task<UserModel?> GetCurrentUserAsync()
    {
        var token = GetAuthToken();
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        SetAuthorizationHeader(token);
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/apiusers/me");
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return null;
        }

        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadFromJsonAsync<UserModel>();
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/v1/apiusers/{user.Id}", user);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to update user");
        }
        
        await EnsureSuccessStatusCodeAsync(response);
        var result = await response.Content.ReadFromJsonAsync<UserModel>();
        
        return result ?? throw new InvalidOperationException("Failed to update user");
    }
    
    public async Task DeleteUserAsync(Guid id)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/apiusers/{id}");
    
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to delete user");
        }
    
        await EnsureSuccessStatusCodeAsync(response);

        DeleteAuthToken();
    }

    
    
    private string? GetAuthToken() => _httpContextAccessor.HttpContext?.Request.Cookies[AuthTokenCookieName];

    private string GetAuthTokenOrThrow() => GetAuthToken() ?? throw new UnauthorizedAccessException("No authentication token found");

    private void SetAuthorizationHeader(string token) => 
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    private void DeleteAuthToken() => 
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(AuthTokenCookieName);

    private static string GetBaseUrl(IConfiguration configuration)
    {
        var isRunningInContainer = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));
        var connectionKey = isRunningInContainer ? "Connections:docker" : "Connections:localhost";
        return configuration.GetValue<string>(connectionKey) ?? throw new InvalidOperationException("Base URL not configured");
    }

    private static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}. Response content: {content}");
        }
    }
}