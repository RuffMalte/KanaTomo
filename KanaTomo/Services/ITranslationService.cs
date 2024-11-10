using KanaTomo.Models;

namespace KanaTomo.Services;

public interface ITranslationService
{
    Task<List<TranslationModel>?> Translate(string text, string targetLanguage);
}