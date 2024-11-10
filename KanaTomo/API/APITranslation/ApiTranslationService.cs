using KanaTomo.Web.Models.Translation;

namespace KanaTomo.API.APITranslation;

public interface IApiTranslationService
{
    List<TranslationModel> Translate(string text, string target);
}



public class ApiTranslationService : IApiTranslationService
{
    private readonly IApiTranslationRepository _repository;
    private readonly ILogger<ApiTranslationService> _logger;

    public ApiTranslationService(IApiTranslationRepository repository, ILogger<ApiTranslationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public List<TranslationModel> Translate(string text, string target)
    {
        _logger.LogInformation($"Translating text: {text} to {target}");
        return _repository.GetTranslations(text, target);
        // Here you could add additional business logic if needed
    }
}
