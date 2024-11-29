using System.Text.Json;
using KanaTomo.Models.Translation;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace KanaTomo.Web.Repositories.Translation;

public class TranslationRepository : ITranslationRepository
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TranslationRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    public async Task<TranslationModel> TranslateAsync(string text)
    {
        var baseUrl = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) 
            ? _configuration.GetValue<string>("Connections:localhost") 
            : _configuration.GetValue<string>("Connections:docker");
        
        var response = await _httpClient.GetAsync($"{baseUrl}/api/v1/apitranslate/translate?text={Uri.EscapeDataString(text)}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var model = JsonSerializer.Deserialize<TranslationModel>(content, options);
        
        return model;
    }
}