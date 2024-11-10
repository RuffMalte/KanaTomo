using KanaTomo.Models;

namespace KanaTomo.Repositories;

public interface ITranslationRepository
{
    Task<List<TranslationModel>> TranslateAsync(string text, string targetLanguage);
}