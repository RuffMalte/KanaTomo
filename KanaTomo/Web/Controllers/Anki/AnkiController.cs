using System.Security.Claims;
using KanaTomo.Models.Anki;
using KanaTomo.ViewModels;
using KanaTomo.Web.Services.Anki;
using KanaTomo.Web.Services.User;

namespace KanaTomo.Web.Controllers.Anki;

using KanaTomo.Models.User;
using Microsoft.AspNetCore.Mvc;

public class AnkiController : Controller
{
    private readonly ILogger<AnkiController> _logger;
    private readonly IAnkiService _ankiService;
    private readonly IUserService _userService;

    public AnkiController(IAnkiService ankiService, ILogger<AnkiController> logger, IUserService userService)
    {
        _logger = logger;
        _ankiService = ankiService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAnkiItemViewModel model)
    {
        _logger.LogError("Adding an Anki card with front: {Front}, back: {Back}", model.Front, model.Back);
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var ankiItem = new AnkiModel()
            {
                Front = model.Front,
                Back = model.Back,
                UserId = currentUser.Id,
            };

            var addedCard = await _ankiService.AddCardToUserAsync(ankiItem);
            TempData["SuccessMessage"] = "Anki card added successfully!";
            _logger.LogInformation("An Anki card with id {Id} was added successfully", addedCard.Id);
            return RedirectToAction("Translate", "Translation");
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding an Anki card");
            ModelState.AddModelError("", "An error occurred while adding the card. Please try again.");
            return RedirectToAction("Translate", "Translation");
        }
    }



    [HttpGet("cards")]
    public async Task<IActionResult> GetUserCards()
    {
        try
        {
            var cards = await _ankiService.GetUserCardsAsync();
            return Ok(cards);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching user's Anki cards");
            return StatusCode(500, "An error occurred while fetching the cards");
        }
    }

    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetCard(Guid id)
    {
        try
        {
            var card = await _ankiService.GetCardByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching an Anki card");
            return StatusCode(500, "An error occurred while fetching the card");
        }
    }
}