using KanaTomo.Models.User;
using Microsoft.EntityFrameworkCore;

namespace KanaTomo.API.APIUser;

public interface IApiUserRepository
{
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(Guid id);
    Task<UserModel> CreateUserAsync(UserModel user);
    Task UpdateUserAsync(UserModel user);
    Task DeleteUserAsync(Guid id);
}

public class ApiUserRepository : IApiUserRepository
{
    private readonly UserContext _context;

    public ApiUserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserModel> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        _context.Entry(user).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(user.Id))
            {
                throw new KeyNotFoundException("User not found");
            }
            else
            {
                throw;
            }
        }
    }

    private async Task<bool> UserExists(Guid id)
    {
        return await _context.Users.AnyAsync(e => e.Id == id);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}

