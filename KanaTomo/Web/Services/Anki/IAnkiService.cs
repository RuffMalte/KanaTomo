using KanaTomo.Models.Anki;

namespace KanaTomo.Web.Services.Anki;

public interface IAnkiService
{
    Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem);
    Task<IEnumerable<AnkiModel>> GetUserCardsAsync();
    Task<AnkiModel?> GetCardByIdAsync(Guid id);
}