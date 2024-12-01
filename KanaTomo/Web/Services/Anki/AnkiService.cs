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

    public async Task<AnkiModel> AddCardToUserAsync(AnkiModel ankiItem)
    {
        return await _ankiRepository.AddCardToUserAsync(ankiItem);
    }

    public async Task<IEnumerable<AnkiModel>> GetUserCardsAsync()
    {
        return await _ankiRepository.GetUserCardsAsync();
    }

    public async Task<AnkiModel?> GetCardByIdAsync(Guid id)
    {
        return await _ankiRepository.GetCardByIdAsync(id);
    }
}