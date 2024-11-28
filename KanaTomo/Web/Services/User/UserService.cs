using KanaTomo.Models.User;
using KanaTomo.Web.Repositories.User;

namespace KanaTomo.Web.Services.User;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel?> GetCurrentUserAsync()
    {
        return await _userRepository.GetCurrentUserAsync();
    }
    
    public async Task<UserModel> UpdateUserAsync(UserModel user)
    {
        return await _userRepository.UpdateUserAsync(user);
    }
}