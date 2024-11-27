using KanaTomo.Models.User;

namespace KanaTomo.Web.Controllers.Auth;

public interface IAuthRepository
{
    Task<string> LoginAsync(string username, string password);
    Task<string> RegisterAsync(string username, string password);
}