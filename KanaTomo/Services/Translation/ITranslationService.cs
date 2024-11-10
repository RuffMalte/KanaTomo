using KanaTomo.Models.Translation;

namespace KanaTomo.Services.Translation;

public interface ITranslationService
{
    Task<List<TranslationModel>?> Translate(string text, string targetLanguage);
}