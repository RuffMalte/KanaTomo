using KanaTomo.Models.Translation;

namespace KanaTomo.API.APITranslation;

public interface IApiTranslationRepository
{
    List<TranslationModel> GetTranslations(string text, string target);
}

public class ApiTranslationRepository : IApiTranslationRepository
{
    private readonly ILogger<ApiTranslationRepository> _logger;

    public ApiTranslationRepository(ILogger<ApiTranslationRepository> logger)
    {
        _logger = logger;
    }

    public List<TranslationModel> GetTranslations(string text, string target)
    {
        _logger.LogInformation($"Fetching translations for text: {text} to {target}");

        // Mock translation logic for demo purposes
        // In a real-world scenario, this might interact with a database or external API
        var translationResult = new List<TranslationModel>
        {
            new TranslationModel(text, target == "Japanese" ? "こんにちは" : "Hello", target),
            new TranslationModel("World", target == "Japanese" ? "世界" : "World", target),
            new TranslationModel("Goodbye", target == "Japanese" ? "さようなら" : "Goodbye", target)
        };

        return translationResult;
    }
}
