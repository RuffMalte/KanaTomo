using Xunit.Abstractions;
using System;
using System.Threading.Tasks;
using KanaTomo.Models.User;
using KanaTomo.Web.Controllers.Auth;
using KanaTomo.Web.Services.User;
using KanaTomo.Web.Repositories.User;
using KanaTomo.Web.Repositories.Auth;
using Moq;
using Xunit;

namespace Tests;

public class ServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly AuthService _authService;
    private readonly ITestOutputHelper _output;

    public ServiceTests(ITestOutputHelper output)
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
        _authRepositoryMock = new Mock<IAuthRepository>();
        _authService = new AuthService(_authRepositoryMock.Object);
        _output = output;
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnCurrentUser()
    {
        _output.WriteLine("Starting GetCurrentUserAsync_ShouldReturnCurrentUser test");

        // Arrange
        var expectedUser = new UserModel { Id = Guid.NewGuid(), Username = "testuser" };
        _userRepositoryMock.Setup(repo => repo.GetCurrentUserAsync())
            .ReturnsAsync(expectedUser);

        _output.WriteLine($"Arranged test with expected user: Id={expectedUser.Id}, Username={expectedUser.Username}");

        // Act
        _output.WriteLine("Calling GetCurrentUserAsync");
        var result = await _userService.GetCurrentUserAsync();

        _output.WriteLine($"Received result: {(result != null ? $"Id={result.Id}, Username={result.Username}" : "null")}");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.Username, result.Username);

        _output.WriteLine("Test completed successfully");
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnUpdatedUser()
    {
        _output.WriteLine("Starting UpdateUserAsync_ShouldReturnUpdatedUser test");

        // Arrange
        var user = new UserModel { Id = Guid.NewGuid(), Username = "updateduser" };
        _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(user))
            .ReturnsAsync(user);

        _output.WriteLine($"Arranged test with user to update: Id={user.Id}, Username={user.Username}");

        // Act
        _output.WriteLine("Calling UpdateUserAsync");
        var result = await _userService.UpdateUserAsync(user);

        _output.WriteLine($"Received result: {(result != null ? $"Id={result.Id}, Username={result.Username}" : "null")}");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Username, result.Username);

        _output.WriteLine("Test completed successfully");
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallDeleteOnRepository()
    {
        _output.WriteLine("Starting DeleteUserAsync_ShouldCallDeleteOnRepository test");

        // Arrange
        var userId = Guid.NewGuid();
        _output.WriteLine($"Arranged test with user ID to delete: {userId}");

        // Act
        _output.WriteLine("Calling DeleteUserAsync");
        await _userService.DeleteUserAsync(userId);

        // Assert
        _output.WriteLine("Verifying DeleteUserAsync was called on repository");
        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);

        _output.WriteLine("Test completed successfully");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken()
    {
        _output.WriteLine("Starting LoginAsync_ShouldReturnToken test");

        // Arrange
        var username = "testuser";
        var password = "testpassword";
        var expectedToken = "test_token";
        _authRepositoryMock.Setup(repo => repo.LoginAsync(username, password))
            .ReturnsAsync(expectedToken);

        _output.WriteLine($"Arranged test with username: {username}");

        // Act
        _output.WriteLine("Calling LoginAsync");
        var result = await _authService.LoginAsync(username, password);

        _output.WriteLine($"Received result: {result}");

        // Assert
        Assert.Equal(expectedToken, result);

        _output.WriteLine("Test completed successfully");
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnToken()
    {
        _output.WriteLine("Starting RegisterAsync_ShouldReturnToken test");

        // Arrange
        var username = "newuser";
        var password = "newpassword";
        var email = "newuser@example.com";
        var expectedToken = "new_user_token";
        _authRepositoryMock.Setup(repo => repo.RegisterAsync(username, password, email))
            .ReturnsAsync(expectedToken);

        _output.WriteLine($"Arranged test with username: {username}, email: {email}");

        // Act
        _output.WriteLine("Calling RegisterAsync");
        var result = await _authService.RegisterAsync(username, password, email);

        _output.WriteLine($"Received result: {result}");

        // Assert
        Assert.Equal(expectedToken, result);

        _output.WriteLine("Test completed successfully");
    }
}