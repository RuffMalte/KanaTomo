using KanaTomo.Models.Anki;

namespace KanaTomo.Web.Repositories.Anki;

public interface IAnkiRepository
{
    Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem);
    Task<IEnumerable<AnkiModel>> GetUserCardsAsync();
    Task<AnkiModel?> GetCardByIdAsync(Guid id);
}