using KanaTomo.Models.Anki;

namespace KanaTomo.API.APIAnki;

public interface IApiAnkiService
{
    Task<AnkiModel> AddCardToUserAsync(Guid userId, AnkiModel ankiItem);
    Task<AnkiModel> GetAnkiItemByIdAsync(Guid id);
    // Add other methods as needed
}

public class ApiAnkiService : IApiAnkiService
{
    private readonly IApiAnkiRepository _ankiRepository;

    public ApiAnkiService(IApiAnkiRepository ankiRepository)
    {
        _ankiRepository = ankiRepository;
    }

    public async Task<AnkiModel> AddCardToUserAsync(Guid userId, AnkiModel ankiItem)
    {
        ankiItem.UserId = userId;
        return await _ankiRepository.CreateAnkiItemAsync(ankiItem);
    }

    public async Task<AnkiModel> GetAnkiItemByIdAsync(Guid id)
    {
        return await _ankiRepository.GetAnkiItemByIdAsync(id);
    }

    // Implement other methods as needed
}