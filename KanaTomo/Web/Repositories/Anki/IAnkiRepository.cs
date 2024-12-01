using KanaTomo.Models.Anki;

namespace KanaTomo.Web.Repositories.Anki;

public interface IAnkiRepository
{
    Task<IEnumerable<AnkiModel>> GetUserAnkiItemsAsync();
    Task<AnkiModel?> GetAnkiItemByIdAsync(Guid id);
    Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem);
    Task<AnkiModel?> UpdateAnkiItemAsync(AnkiModel ankiItem);
    Task<bool> DeleteAnkiItemAsync(Guid id);
}