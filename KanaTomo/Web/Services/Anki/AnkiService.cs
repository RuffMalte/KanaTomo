using KanaTomo.Models.Anki;
using KanaTomo.Web.Repositories.Anki;

namespace KanaTomo.Web.Services.Anki;

public class AnkiService : IAnkiService
{
    private readonly IAnkiRepository _ankiRepository;

    public AnkiService(IAnkiRepository ankiRepository)
    {
        _ankiRepository = ankiRepository;
    }

    public async Task<IEnumerable<AnkiModel>> GetUserAnkiItemsAsync()
    {
        return await _ankiRepository.GetUserAnkiItemsAsync();
    }

    public async Task<AnkiModel?> GetAnkiItemByIdAsync(Guid id)
    {
        return await _ankiRepository.GetAnkiItemByIdAsync(id);
    }

    public async Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem)
    {
        return await _ankiRepository.AddCardToUserAsync(ankiItem);
    }

    public async Task<AnkiModel?> UpdateAnkiItemAsync(AnkiModel ankiItem)
    {
        return await _ankiRepository.UpdateAnkiItemAsync(ankiItem);
    }

    public async Task<bool> DeleteAnkiItemAsync(Guid id)
    {
        return await _ankiRepository.DeleteAnkiItemAsync(id);
    }
    
    public async Task<IEnumerable<AnkiModel>> GetDueAnkiItemsAsync()
    {
        return await _ankiRepository.GetDueAnkiItemsAsync();
    }
    
    public async Task<AnkiModel> ReviewAnkiItemAsync(Guid id, int difficulty)
    {
        return await _ankiRepository.ReviewAnkiItemAsync(id, difficulty);
    }
}