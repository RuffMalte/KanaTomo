using KanaTomo.Models.Translation;

namespace KanaTomo.API.APITranslation;

public interface IApiTranslationService
{
    Task<TranslationModel>Translate(string text);
}



public class ApiTranslationService : IApiTranslationService
{
    private readonly IApiTranslationRepository _repository;

    public ApiTranslationService(IApiTranslationRepository repository)
    {
        _repository = repository;
    }

    public async Task<TranslationModel> Translate(string text)
    {
        return await _repository.GetTranslationsAsync(text);
    }
}
