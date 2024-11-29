using KanaTomo.API.APIAuth;
using KanaTomo.Models.Auth;
using KanaTomo.Models.User;

namespace KanaTomo.Web.Controllers.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AuthRepository(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = GetBaseUrl(configuration);
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        return await SendAuthRequestAsync<LoginDto>("login", new LoginDto { Username = username, Password = password });
    }

    public async Task<string> RegisterAsync(string username, string password, string email)
    {
        return await SendAuthRequestAsync<RegisterDto>("register", new RegisterDto { Username = username, Password = password, Email = email });
    }

    private async Task<string> SendAuthRequestAsync<T>(string endpoint, T dto) where T : class
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/apiauth/{endpoint}", dto);
        await EnsureSuccessStatusCodeAsync(response);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result?.Token ?? throw new InvalidOperationException("Token not received");
    }

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