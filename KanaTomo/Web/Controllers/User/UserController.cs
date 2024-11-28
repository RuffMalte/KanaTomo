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
            return RedirectToAction("Error", "Home");
        } 
    }
    
    
    [HttpGet("edit")]
    public async Task<IActionResult> Edit()
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the current user for editing");
            return RedirectToAction("Error", "Home");
        }
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> Update(UserModel user)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Only update allowed fields
            currentUser.Username = user.Username;
            currentUser.Email = user.Email;
            currentUser.Bio = user.Bio;
            currentUser.ProfilePictureUrl = user.ProfilePictureUrl;

            var updatedUser = await _userService.UpdateUserAsync(currentUser);
            return RedirectToAction("Me");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user");
            ModelState.AddModelError(string.Empty, "Update failed: " + ex.Message);
            return View("Edit", user);
        }
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete()
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }
        
            await _userService.DeleteUserAsync(currentUser.Id);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the current user");
            return RedirectToAction("Error", "Home");
        }
    }
}