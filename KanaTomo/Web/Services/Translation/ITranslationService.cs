using KanaTomo.Models.Translation;

namespace KanaTomo.Web.Services.Translation;

public interface ITranslationService
{
    Task<List<TranslationModel>?> Translate(string text, string targetLanguage);
}