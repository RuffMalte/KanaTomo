using KanaTomo.Web.Models.Translation;

namespace KanaTomo.Web.Repositories.Translation;

public interface ITranslationRepository
{
    Task<List<TranslationModel>> TranslateAsync(string text, string targetLanguage);
}