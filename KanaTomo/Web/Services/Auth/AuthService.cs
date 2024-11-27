using KanaTomo.Models.User;
using KanaTomo.Web.Controllers.Auth;

namespace KanaTomo.Web.Repositories.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        return await _authRepository.LoginAsync(username, password);
    }

    public async Task<string> RegisterAsync(string username, string password)
    {
        return await _authRepository.RegisterAsync(username, password);
    }
}