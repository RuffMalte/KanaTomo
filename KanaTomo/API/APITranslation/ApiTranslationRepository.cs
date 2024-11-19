using System.Net.Http;
using DeepL;
using DeepL.Model;
using KanaTomo.Models.Translation;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    private IConfiguration configuration;

    public ApiTranslationRepository(ILogger<ApiTranslationRepository> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task<TranslationModel> GetTranslationsAsync(string text, string target)
    {
        _logger.LogInformation($"Fetching translations for text: {text} to {target}");

        var translationModel = new TranslationModel(text, target);
    
        try
        {
            var deeplResult = await GetDeeplTranslationAsync(text);
            translationModel.DeeplResponse = new DeeplResponseModel() 
            {
                Text = deeplResult.Text,
                DetectedSourceLanguage = deeplResult.DetectedSourceLanguageCode,
                BilledCharacters = deeplResult.BilledCharacters
            };
            
            translationModel.JishoResponse = await GetJishoTranslationAsync(text);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while fetching DeepL translation");
            throw new Exception($"Error in DeepL translation: {e.Message}", e);
        }
    
        return translationModel;
    }

    private async Task<JishoResponse> GetJishoTranslationAsync(string keyword)
    {
        var response = await _httpClient.GetStringAsync($"{JishoApiUrl}{Uri.EscapeDataString(keyword)}");
        return JsonSerializer.Deserialize<JishoResponse>(response);
    }

    private async Task<TextResult> GetDeeplTranslationAsync(string text)
    {
        var formailties = new[] { Formality.PreferLess, Formality.Less, Formality.Default, Formality.More, Formality.PreferMore };
        
        var deepLApiKey = this.configuration["deeplApiKey"] ?? Environment.GetEnvironmentVariable("deeplApiKey") ?? string.Empty;
        var translator = new Translator(deepLApiKey);
        var result = await translator.TranslateTextAsync(
            text,
            LanguageCode.English,
            LanguageCode.Japanese,
            new TextTranslateOptions { Formality = formailties[4] }
        );
        
        return result;
    }
}
