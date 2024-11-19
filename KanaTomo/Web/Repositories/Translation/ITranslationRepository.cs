using KanaTomo.Models.Translation;

namespace KanaTomo.Web.Repositories.Translation;

public interface ITranslationRepository
{
    Task<TranslationModel> TranslateAsync(string text);
}