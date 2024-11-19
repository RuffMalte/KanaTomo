using System.Net.Http;
using DeepL;
using DeepL.Model;
using KanaTomo.Helper;
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
            if (deeplResult != null)
            {
                translationModel.DeeplResponse = new DeeplResponseModel() 
                {
                    Text = deeplResult.Text,
                    DetectedSourceLanguage = deeplResult.DetectedSourceLanguageCode,
                    BilledCharacters = deeplResult.BilledCharacters
                };
            }
            
            var jishoResult = await GetJishoTranslationAsync(text);
            if (jishoResult != null)
            {
                translationModel.JishoResponse = jishoResult;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while fetching DeepL translation");
            throw new Exception($"Error in DeepL translation: {e.Message}", e);
        }
    
        return translationModel;
    }

    private async Task<JishoResponse?> GetJishoTranslationAsync(string keyword)
    {
        //needs no lng detection, because the api will.
        var response = await _httpClient.GetStringAsync($"{JishoApiUrl}{Uri.EscapeDataString(keyword)}");
        if (string.IsNullOrWhiteSpace(response))
        {
            _logger.LogError("Jisho API returned an empty response.");
            return null;
        }
        return JsonSerializer.Deserialize<JishoResponse>(response);
    }

    private async Task<TextResult?> GetDeeplTranslationAsync(string text)
    {
        var formailties = new[] { Formality.PreferLess, Formality.Less, Formality.Default, Formality.More, Formality.PreferMore };
        
        var deepLApiKey = this.configuration["deeplApiKey"] ?? Environment.GetEnvironmentVariable("deeplApiKey") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(deepLApiKey))
        {
            _logger.LogError("DeepL API key is not set. Please set the deeplApiKey environment variable, but does not have to be set, if you do not want to use DeepL translation.");
            return null;
        }

        var languageDetector = new LanguageFinder();
        var sourceLanguage = languageDetector.DetectLanguage(text);
    
        if (sourceLanguage == null)
        {
            _logger.LogError("Unable to detect language");
            return null;
        }

        _logger.LogInformation("Detected language: " + sourceLanguage);
        
        var sourceLanguageCode = sourceLanguage == "en-US" ? LanguageCode.English : LanguageCode.Japanese;
        _logger.LogInformation("Source language code: " + sourceLanguageCode);
        var targetLanguageCode = sourceLanguage == "en-US" ? LanguageCode.Japanese : LanguageCode.EnglishAmerican;
        _logger.LogInformation("Target language code: " + targetLanguageCode);
        
        try
        {
            var translator = new Translator(deepLApiKey);
            var result = await translator.TranslateTextAsync(
                text,
                sourceLanguageCode,
                targetLanguageCode,
                new TextTranslateOptions { Formality = formailties[4] }
            );
            _logger.LogInformation(JsonSerializer.Serialize(result));

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
}
