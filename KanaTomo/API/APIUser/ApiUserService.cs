using KanaTomo.Models.User;

namespace KanaTomo.API.APIUser;

public interface IApiUserService
{
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(Guid id);
    Task<UserModel> CreateUserAsync(UserModel user);
    Task<UserModel> UpdateUserAsync(UserModel user);
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

    public async Task<UserModel> UpdateUserAsync(UserModel user)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        // Update only the fields that are allowed to be changed
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Bio = user.Bio;
        existingUser.ProfilePictureUrl = user.ProfilePictureUrl;

        await _userRepository.UpdateUserAsync(existingUser);
        return existingUser;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _userRepository.DeleteUserAsync(id);
    }
}