using KanaTomo.Models.User;

namespace KanaTomo.Web.Repositories.User;

public interface IUserRepository
{
    Task<UserModel> GetCurrentUserAsync();
}