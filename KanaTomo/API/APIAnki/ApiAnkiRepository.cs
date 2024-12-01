using KanaTomo.Models.Anki;
using KanaTomo.Models.User;

namespace KanaTomo.API.APIAnki;

using Microsoft.EntityFrameworkCore;

public interface IApiAnkiRepository
{
    Task<AnkiModel> CreateAnkiItemAsync(AnkiModel ankiItem);
    Task<AnkiModel> GetAnkiItemByIdAsync(Guid id);
    // Add other methods as needed
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

    // Implement other methods as needed
}