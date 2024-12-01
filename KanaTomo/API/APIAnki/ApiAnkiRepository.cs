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

    public async Task<AnkiModel> UpdateAnkiItemAsync(AnkiModel ankiItem)
    {
        _context.Entry(ankiItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ankiItem;
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
}