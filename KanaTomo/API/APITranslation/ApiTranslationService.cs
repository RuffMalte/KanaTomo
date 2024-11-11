using KanaTomo.Models.Translation;

namespace KanaTomo.API.APITranslation;

public interface IApiTranslationService
{
    Task<TranslationModel>Translate(string text, string target);
}



public class ApiTranslationService : IApiTranslationService
{
    private readonly IApiTranslationRepository _repository;

    public ApiTranslationService(IApiTranslationRepository repository)
    {
        _repository = repository;
    }

    public async Task<TranslationModel> Translate(string text, string targetLanguage)
    {
        return await _repository.GetTranslationsAsync(text, targetLanguage);
    }
}
