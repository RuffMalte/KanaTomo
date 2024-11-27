using KanaTomo.Models.User;
using Microsoft.EntityFrameworkCore;

namespace KanaTomo.API.APIAuth;

public interface IApiAuthRepository
{
    Task<UserModel> GetUserByUsernameAsync(string username);
    Task<UserModel> CreateUserAsync(UserModel user, string password);
}

public class ApiAuthRepository : IApiAuthRepository
{
    private readonly UserContext _context;

    public ApiAuthRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<UserModel> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user, string password)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}