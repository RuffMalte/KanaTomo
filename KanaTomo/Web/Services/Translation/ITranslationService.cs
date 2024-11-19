using KanaTomo.Models.Translation;

namespace KanaTomo.Web.Services.Translation;

public interface ITranslationService
{
    Task<TranslationModel?> Translate(string text);
}