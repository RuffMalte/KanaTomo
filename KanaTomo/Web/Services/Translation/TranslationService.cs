using KanaTomo.Models.Translation;
using KanaTomo.Web.Repositories.Translation;

namespace KanaTomo.Web.Services.Translation;

public class TranslationService : ITranslationService
{
    private readonly ITranslationRepository _repository;

    public TranslationService(ITranslationRepository repository)
    {
        _repository = repository;
    }

    public async Task<TranslationModel> Translate(string text)
    {
        return await _repository.TranslateAsync(text);
    }
}