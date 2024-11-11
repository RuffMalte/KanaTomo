using System.Net.Http;
using System.Text.Json;
using KanaTomo.Models.Translation;

namespace KanaTomo.API.APITranslation;

public interface IApiTranslationRepository
{
    Task<TranslationModel> GetTranslationsAsync(string text, string target);
}

public class ApiTranslationRepository : IApiTranslationRepository
{
    private readonly ILogger<ApiTranslationRepository> _logger;
    private readonly HttpClient _httpClient;
    private const string JishoApiUrl = "https://jisho.org/api/v1/search/words?keyword=";

    public ApiTranslationRepository(ILogger<ApiTranslationRepository> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<TranslationModel> GetTranslationsAsync(string text, string target)
    {
        _logger.LogInformation($"Fetching translations for text: {text} to {target}");

        var translationModel = new TranslationModel(text, target);
        translationModel.JishoResponse = await GetJishoTranslationAsync(text);

        return translationModel;
    }

    private async Task<JishoResponse> GetJishoTranslationAsync(string keyword)
    {
        var response = await _httpClient.GetStringAsync($"{JishoApiUrl}{Uri.EscapeDataString(keyword)}");
        return JsonSerializer.Deserialize<JishoResponse>(response);
    }
}
