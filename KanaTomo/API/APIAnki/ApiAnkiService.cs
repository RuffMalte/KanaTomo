using KanaTomo.Models.Anki;

namespace KanaTomo.API.APIAnki;

public interface IApiAnkiService
{
    Task<AnkiModel> AddCardToUserAsync(Guid userId, AnkiModel ankiItem);
    Task<AnkiModel> GetAnkiItemByIdAsync(Guid id);
    Task<IEnumerable<AnkiModel>> GetAnkiItemsByUserIdAsync(Guid userId);
    Task<AnkiModel> UpdateAnkiItemAsync(AnkiModel ankiItem);
    Task<bool> DeleteAnkiItemAsync(Guid id);
    Task<IEnumerable<AnkiModel>> GetDueAnkiItemsByUserIdAsync(Guid userId);
    Task<AnkiModel> UpdateCardAfterReviewAsync(Guid id, int difficulty);
    Task<bool> ResetAllCardsForUserAsync(Guid userId);
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

    public async Task<IEnumerable<AnkiModel>> GetAnkiItemsByUserIdAsync(Guid userId)
    {
        return await _ankiRepository.GetAnkiItemsByUserIdAsync(userId);
    }

    public async Task<AnkiModel> UpdateAnkiItemAsync(AnkiModel ankiItem)
    {
        return await _ankiRepository.UpdateAnkiItemAsync(ankiItem);
    }

    public async Task<bool> DeleteAnkiItemAsync(Guid id)
    {
        return await _ankiRepository.DeleteAnkiItemAsync(id);
    }
    
    public async Task<IEnumerable<AnkiModel>> GetDueAnkiItemsByUserIdAsync(Guid userId)
    {
        var currentDate = DateTime.UtcNow;
        return await _ankiRepository.GetDueAnkiItemsByUserIdAsync(userId, currentDate);
    }
    
    public async Task<AnkiModel> UpdateCardAfterReviewAsync(Guid id, int difficulty)
    {
        return await _ankiRepository.UpdateCardAfterReviewAsync(id, difficulty);
    }
    
    public async Task<bool> ResetAllCardsForUserAsync(Guid userId)
    {
        return await _ankiRepository.ResetAllCardsForUserAsync(userId);
    }
}