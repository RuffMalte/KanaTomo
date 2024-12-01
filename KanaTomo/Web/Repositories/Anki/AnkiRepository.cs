using System.Net;
using System.Net.Http.Headers;
using KanaTomo.Models.Anki;


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

    public async Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/apianki/addCard", ankiItem);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to add card");
        }
        
        await EnsureSuccessStatusCodeAsync(response);
        var result = await response.Content.ReadFromJsonAsync<AnkiModel>();
        
        return result ?? throw new InvalidOperationException("Failed to add card");
    }

    public async Task<IEnumerable<AnkiModel>> GetUserCardsAsync()
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/apianki/cards");
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to fetch cards");
        }
        
        await EnsureSuccessStatusCodeAsync(response);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<AnkiModel>>();
        
        return result ?? Enumerable.Empty<AnkiModel>();
    }

    public async Task<AnkiModel?> GetCardByIdAsync(Guid id)
    {
        var token = GetAuthTokenOrThrow();
        SetAuthorizationHeader(token);

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/apianki/card/{id}");
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized to fetch card");
        }
        
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        
        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadFromJsonAsync<AnkiModel>();
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