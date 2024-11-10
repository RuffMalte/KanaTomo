using KanaTomo.Models.Translation;

namespace KanaTomo.Repositories.Translation;

public interface ITranslationRepository
{
    Task<List<TranslationModel>> TranslateAsync(string text, string targetLanguage);
}