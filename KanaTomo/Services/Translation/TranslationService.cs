using KanaTomo.Models.Translation;
using KanaTomo.Repositories.Translation;

namespace KanaTomo.Services.Translation;

public class TranslationService : ITranslationService
{
    private readonly ITranslationRepository _repository;

    public TranslationService(ITranslationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TranslationModel>> Translate(string text, string targetLanguage)
    {
        return await _repository.TranslateAsync(text, targetLanguage);
    }
}