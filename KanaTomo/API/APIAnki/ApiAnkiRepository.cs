using KanaTomo.Models.Anki;
using KanaTomo.Models.User;

namespace KanaTomo.API.APIAnki;

using Microsoft.EntityFrameworkCore;

public interface IApiAnkiRepository
{
    Task<AnkiModel> CreateAnkiItemAsync(AnkiModel ankiItem);
    Task<AnkiModel> GetAnkiItemByIdAsync(Guid id);
    Task<IEnumerable<AnkiModel>> GetAnkiItemsByUserIdAsync(Guid userId);
    Task<AnkiModel> UpdateAnkiItemAsync(AnkiModel ankiItem);
    Task<bool> DeleteAnkiItemAsync(Guid id);
    Task<IEnumerable<AnkiModel>> GetDueAnkiItemsByUserIdAsync(Guid userId, DateTime currentDate);
    
    Task<AnkiModel> UpdateCardAfterReviewAsync(Guid id, int difficulty);
    
    Task<bool> ResetAllCardsForUserAsync(Guid userId);
}

public class ApiAnkiRepository : IApiAnkiRepository
{
    private readonly UserContext _context;

    public ApiAnkiRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<AnkiModel> CreateAnkiItemAsync(AnkiModel ankiItem)
    {
        _context.AnkiItems.Add(ankiItem);
        await _context.SaveChangesAsync();
        return ankiItem;
    }

    public async Task<AnkiModel> GetAnkiItemByIdAsync(Guid id)
    {
        return await _context.AnkiItems.FindAsync(id);
    }

    public async Task<IEnumerable<AnkiModel>> GetAnkiItemsByUserIdAsync(Guid userId)
    {
        return await _context.AnkiItems.Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task<AnkiModel?> UpdateAnkiItemAsync(AnkiModel ankiItem)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == ankiItem.UserId);
        if (!userExists)
        {
            return null; // Or throw a custom exception
        }
        
        var existingItem = await _context.AnkiItems.FindAsync(ankiItem.Id);
        if (existingItem == null)
        {
            return null;
        }
        
        existingItem.Front = ankiItem.Front;
        existingItem.Back = ankiItem.Back;
        existingItem.ReviewCount = ankiItem.ReviewCount;
        existingItem.CreatedAt = ankiItem.CreatedAt;

        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task<bool> DeleteAnkiItemAsync(Guid id)
    {
        var ankiItem = await _context.AnkiItems.FindAsync(id);
        if (ankiItem == null)
        {
            return false;
        }

        _context.AnkiItems.Remove(ankiItem);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<AnkiModel>> GetDueAnkiItemsByUserIdAsync(Guid userId, DateTime currentDate)
    {
        return await _context.AnkiItems
            .Where(a => a.UserId == userId && a.NextReviewDate <= currentDate)
            .ToListAsync();
    }
    
    public async Task<AnkiModel> UpdateCardAfterReviewAsync(Guid id, int difficulty)
    {
        var card = await _context.AnkiItems.FindAsync(id);
        if (card == null)
        {
            return null;
        }

        UpdateCardWithAnkiAlgorithm(card, difficulty);
        await _context.SaveChangesAsync();
        return card;
    }

    private void UpdateCardWithAnkiAlgorithm(AnkiModel card, int difficulty)
    {
        // Ensure difficulty is between 1 and 4
        difficulty = Math.Clamp(difficulty, 1, 4);

        // Calculate new easiness factor
        card.Easiness = Math.Max(1.3, card.Easiness + 0.1 - (5 - difficulty) * (0.08 + (5 - difficulty) * 0.02));

        // Update repetition number
        if (difficulty < 3)
        {
            card.RepetitionNumber = 0;
        }
        else
        {
            card.RepetitionNumber++;
        }

        // Calculate new interval
        if (card.RepetitionNumber == 0)
        {
            card.Interval = 1;
        }
        else if (card.RepetitionNumber == 1)
        {
            card.Interval = 6;
        }
        else
        {
            card.Interval = (int)Math.Round(card.Interval * card.Easiness);
        }

        // Update review dates
        card.LastReviewDate = DateTime.UtcNow;
        card.NextReviewDate = card.LastReviewDate.AddDays(card.Interval);
        card.ReviewCount++;
    }
    
    public async Task<bool> ResetAllCardsForUserAsync(Guid userId)
    {
        var userCards = await _context.AnkiItems.Where(a => a.UserId == userId).ToListAsync();
        foreach (var card in userCards)
        {
            card.NextReviewDate = DateTime.UtcNow;
            card.LastReviewDate = DateTime.UtcNow;
            card.ReviewCount = 0;
            card.Easiness = 2.5;
            card.Interval = 0;
            card.RepetitionNumber = 0;
        }
        await _context.SaveChangesAsync();
        return true;
    }
}