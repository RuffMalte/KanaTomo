using KanaTomo.Helpers;
using KanaTomo.Models;
using Newtonsoft.Json;

namespace KanaTomo.Services;

public class TranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public TranslationService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IWebHostEnvironment env)
    {
        _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5070/api/v1/translate";

        if (env.IsDevelopment())
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
        }
        else
        {
            _httpClient = httpClientFactory.CreateClient();
        }
    }

    public async Task<List<TranslationModel>> Translate(string text, string targetLanguage)
    {
        var response = await _httpClient.GetAsync(
            $"{_apiBaseUrl}/translate?text={Uri.EscapeDataString(text)}&target={targetLanguage}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<TranslationModel>>(content);
    }
}