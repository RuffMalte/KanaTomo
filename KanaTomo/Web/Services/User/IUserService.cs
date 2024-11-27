using KanaTomo.Models.User;

namespace KanaTomo.Web.Services.User;

public interface IUserService
{
    Task<UserModel> GetCurrentUserAsync();
}