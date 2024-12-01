using KanaTomo.Models.Anki;

namespace KanaTomo.Web.Services.Anki;
public interface IAnkiService

{
    Task<IEnumerable<AnkiModel>> GetUserAnkiItemsAsync();
    Task<AnkiModel?> GetAnkiItemByIdAsync(Guid id);
    Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem);
    Task<AnkiModel?> UpdateAnkiItemAsync(AnkiModel ankiItem);
    Task<bool> DeleteAnkiItemAsync(Guid id);
}