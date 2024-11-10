using KanaTomo.Services.Translation;
using KanaTomo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.Controllers.Translation;
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
                model.TranslationResults = await _translationService.Translate(model.TextToTranslate, model.TargetLanguage);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"An error occurred while translating: {ex.Message}");
            }
        }
        return View("Index", model);
    }
}