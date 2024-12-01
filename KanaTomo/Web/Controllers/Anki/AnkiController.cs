using System.Text.Json;
using KanaTomo.Models.Anki;
using KanaTomo.ViewModels.Anki;
using KanaTomo.Web.Services.Anki;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.Web.Controllers.Anki;

public class AnkiController : Controller
{
    private readonly ILogger<AnkiController> _logger;
    private readonly IAnkiService _ankiService;

    public AnkiController(IAnkiService ankiService, ILogger<AnkiController> logger)
    {
        _logger = logger;
        _ankiService = ankiService;
    }

    [HttpGet("cards")]
    public async Task<IActionResult> GetUserCards()
    {
        try
        {
            var allCards = await _ankiService.GetUserAnkiItemsAsync();
            var dueCards = await _ankiService.GetDueAnkiItemsAsync();
            
            var viewModel = new AnkiCardListViewModel 
            { 
                Cards = allCards.ToList(),
                DueCardsCount = dueCards.Count(),
                TotalCardsCount = allCards.Count()
            };
            
            return View(viewModel);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching user's Anki cards");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetCard(Guid id)
    {
        try
        {
            var ankiItem = await _ankiService.GetAnkiItemByIdAsync(id);
            if (ankiItem == null)
            {
                return NotFound();
            }
            return View(ankiItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while fetching Anki card with id {id}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet("card/add")]
    public IActionResult AddCard()
    {
        return View(new AnkiModel());
    }

    [HttpPost("card/add")]
    public async Task<IActionResult> AddCard(AnkiModel ankiItem)
    {
        try
        {
            var createdAnkiItem = await _ankiService.AddCardToUserAsync(ankiItem);
            return RedirectToAction("GetUserCards");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new Anki card");
            ModelState.AddModelError(string.Empty, "Failed to add card: " + ex.Message);
            return View(ankiItem);
        }
    }

    [HttpGet("card/edit/{id}")]
    public async Task<IActionResult> EditCard(Guid id)
    {
        try
        {
            var ankiItem = await _ankiService.GetAnkiItemByIdAsync(id);
            if (ankiItem == null)
            {
                return NotFound();
            }
            return View(ankiItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while fetching Anki card with id {id} for editing");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost("card/edit/{id}")]
    public async Task<IActionResult> EditCard(Guid id, AnkiModel ankiItem)
    {
        if (id != ankiItem.Id)
        {
            return BadRequest();
        }

        try
        {
            var updatedItem = await _ankiService.UpdateAnkiItemAsync(ankiItem);
            if (updatedItem == null)
            {
                return NotFound();
            }
            return RedirectToAction("GetUserCards");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating Anki card with id {id}");
            ModelState.AddModelError(string.Empty, "Update failed: " + ex.Message);
            return View(ankiItem);
        }
    }

    [HttpPost("card/delete/{id}")]
    public async Task<IActionResult> DeleteCard(Guid id)
    {
        try
        {
            var result = await _ankiService.DeleteAnkiItemAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(GetUserCards));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting Anki card with id {id}");
            return RedirectToAction("Error", "Home");
        }
    }
    
    [HttpGet("review")]
    public async Task<IActionResult> Review()
    {
        try
        {
            var dueAnkiItems = await _ankiService.GetDueAnkiItemsAsync();
            if (!dueAnkiItems.Any())
            {
                return View("NoCardsToReview");
            }
            return View(dueAnkiItems.First());
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching due Anki cards");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost("review/{id}")]
    public async Task<IActionResult> ReviewCard(Guid id, int difficulty)
    {
        try
        {
            await _ankiService.ReviewAnkiItemAsync(id, difficulty);
            return RedirectToAction(nameof(Review));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while reviewing Anki card with id {id}");
            return RedirectToAction("Error", "Home");
        }
    }
    
    [HttpPost("reset")]
    public async Task<IActionResult> ResetAllCards()
    {
        try
        {
            var result = await _ankiService.ResetAllCardsAsync();
            if (result)
            {
                TempData["SuccessMessage"] = "All cards have been reset successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to reset cards.";
            }
            return RedirectToAction(nameof(GetUserCards));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while resetting all Anki cards");
            TempData["ErrorMessage"] = "An error occurred while resetting cards.";
            return RedirectToAction(nameof(GetUserCards));
        }
    }
}