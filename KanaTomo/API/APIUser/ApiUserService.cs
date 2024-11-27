using KanaTomo.Models.User;

namespace KanaTomo.API.APIUser;

public interface IApiUserService
{
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(Guid id);
    Task<UserModel> CreateUserAsync(UserModel user);
    Task UpdateUserAsync(UserModel user);
    Task DeleteUserAsync(Guid id);
}

public class ApiUserService : IApiUserService
{
    private readonly IApiUserRepository _userRepository;

    public ApiUserService(IApiUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<UserModel> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user)
    {
        user.CreatedAt = DateTime.UtcNow;
        return await _userRepository.CreateUserAsync(user);
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _userRepository.DeleteUserAsync(id);
    }
}