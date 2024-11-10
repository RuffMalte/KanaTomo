using KanaTomo.Models.Translation;
using Newtonsoft.Json;

namespace KanaTomo.Repositories.Translation;

public class TranslationRepository : ITranslationRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public TranslationRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5070/api/v1/translate";
    }

    public async Task<List<TranslationModel>> TranslateAsync(string text, string targetLanguage)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/translate?text={Uri.EscapeDataString(text)}&target={targetLanguage}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<TranslationModel>>(content);
    }
}