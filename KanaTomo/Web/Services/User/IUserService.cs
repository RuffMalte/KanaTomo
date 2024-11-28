using KanaTomo.Models.User;

namespace KanaTomo.Web.Services.User;

public interface IUserService
{
    Task<UserModel?> GetCurrentUserAsync();
    
    Task<UserModel> UpdateUserAsync(UserModel user);
    
    Task DeleteUserAsync(Guid id);
}