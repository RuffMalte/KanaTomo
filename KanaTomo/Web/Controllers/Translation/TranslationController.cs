using KanaTomo.ViewModels;
using KanaTomo.Web.Services.Translation;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.Web.Controllers.Translation;
public class TranslationController : Controller
{
    private readonly ITranslationService _translationService;

    public TranslationController(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new TranslationViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Translate(TranslationViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                model.TranslationResult = await _translationService.Translate(model.TextToTranslate);
                TempData["LastSearch"] = model.TextToTranslate;
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"An error occurred while translating: {ex.Message}");
            }
        }

        return View("Index", model);
    }
}