using System.Diagnostics;
using KanaTomo.ViewModels;
using KanaTomo.Web.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.Web.Controllers.Home;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    
    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                // Clear the invalid token
                Response.Cookies.Delete("AuthToken");
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}