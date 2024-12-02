using KanaTomo.API.APIEmailService;
using KanaTomo.Models.User;
using Microsoft.EntityFrameworkCore;

namespace KanaTomo.API.APIAuth;

public interface IApiAuthRepository
{
    Task<UserModel> GetUserByUsernameAsync(string username);
    Task<UserModel> CreateUserAsync(UserModel user, string password, string email);
}

public class ApiAuthRepository : IApiAuthRepository
{
    private readonly UserContext _context;
    private readonly IApiEmailService _emailService;

    public ApiAuthRepository(UserContext context, IApiEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<UserModel> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user, string password, string email)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        await _emailService.SendEmailAsync(email, "Welcome to KanaTomo", "Welcome to KanaTomo! We're excited to have you on board.");
        
        return user;
    }
}