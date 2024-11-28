using KanaTomo.Models.User;

namespace KanaTomo.Web.Repositories.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(string username, string password);
    Task<string> RegisterAsync(string username, string password, string email);
}