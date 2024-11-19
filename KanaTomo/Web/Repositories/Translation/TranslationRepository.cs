using KanaTomo.Models.Translation;
using Newtonsoft.Json;

namespace KanaTomo.Web.Repositories.Translation;

public class TranslationRepository : ITranslationRepository
{
    private readonly HttpClient _httpClient;

    public TranslationRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<TranslationModel> TranslateAsync(string text, string targetLanguage)
    {
        var baseUrl = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) ? "http://localhost:5070" : "http://host.docker.internal:5070";
        
        var response = await _httpClient.GetAsync($"{baseUrl}/api/v1/translate/translate?text={Uri.EscapeDataString(text)}&target={targetLanguage}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        
        var model = JsonConvert.DeserializeObject<TranslationModel>(content, settings);
        return model;
    }
}