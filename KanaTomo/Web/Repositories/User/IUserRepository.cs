using KanaTomo.Models.User;
using KanaTomo.Web.Services.User;

namespace KanaTomo.Web.Repositories.User;

public interface IUserRepository
{
    Task<UserModel> GetCurrentUserAsync();
    
    Task<UserModel?> UpdateUserAsync(UserModel user);
    
    Task DeleteUserAsync(Guid id);
}