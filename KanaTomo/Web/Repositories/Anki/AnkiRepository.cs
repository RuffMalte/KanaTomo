using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using KanaTomo.Models.Anki;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace KanaTomo.Web.Repositories.Anki;

public class AnkiRepository : IAnkiRepository
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _baseUrl;
    private const string AuthTokenCookieName = "AuthToken";

    public AnkiRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _baseUrl = GetBaseUrl(configuration);
    }

    public async Task<IEnumerable<AnkiModel>> GetUserAnkiItemsAsync()
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/apianki/user");
        await EnsureSuccessStatusCodeAsync(response);

        return await response.Content.ReadFromJsonAsync<IEnumerable<AnkiModel>>() ?? Enumerable.Empty<AnkiModel>();
    }

    public async Task<AnkiModel?> GetAnkiItemByIdAsync(Guid id)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/apianki/{id}");
        
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadFromJsonAsync<AnkiModel>();
    }

    public async Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        Console.WriteLine(JsonSerializer.Serialize(ankiItem));
        
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/apianki/addCard", ankiItem);
        await EnsureSuccessStatusCodeAsync(response);

        var createdAnkiItem = await response.Content.ReadFromJsonAsync<AnkiModel>();
        return createdAnkiItem ?? throw new InvalidOperationException("Failed to create Anki item");
    }

    public async Task<AnkiModel?> UpdateAnkiItemAsync(AnkiModel ankiItem)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/v1/apianki/{ankiItem.Id}", ankiItem);
        
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessStatusCodeAsync(response);
        return ankiItem;
    }

    public async Task<bool> DeleteAnkiItemAsync(Guid id)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/apianki/{id}");
        
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        await EnsureSuccessStatusCodeAsync(response);
        return true;
    }

    private string? GetAuthToken() => _httpContextAccessor.HttpContext?.Request.Cookies[AuthTokenCookieName];

    private string GetAuthTokenOrThrow() => GetAuthToken() ?? throw new UnauthorizedAccessException("No authentication token found");

    private void SetAuthorizationHeader(string token) => 
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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