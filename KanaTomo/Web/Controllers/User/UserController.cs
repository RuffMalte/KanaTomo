using KanaTomo.Models.User;
using KanaTomo.Web.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KanaTomo.Web.Controllers.User;

public class UserController: Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(IUserService authService, ILogger<UserController> logger)
    {
        _logger = logger;
        _userService = authService;
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(currentUser);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Redirect to login page if unauthorized
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the current user");
            return RedirectToAction("Error");
        } 
    }
    
    [Authorize]
    [HttpPost("update")]
    public async Task<IActionResult> Update(UserModel user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Update failed: " + ex.Message);
            }
        }

        return RedirectToAction("Index", "Home");
    }
}