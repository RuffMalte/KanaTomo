using KanaTomo.Models.Translation;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.API.APITranslation;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class TranslateController : ControllerBase
{
    private readonly IApiTranslationService _translationService;
    private readonly ILogger<TranslateController> _logger;

    public TranslateController(IApiTranslationService translationService, ILogger<TranslateController> logger)
    {
        _translationService = translationService;
        _logger = logger;
    }

    [HttpGet("helloworld")]
    public ActionResult<string> HelloWorld()
    {
        return "Hello World";
    }

    [HttpGet("translate")]
    public ActionResult<List<TranslationModel>> Translate([FromQuery] string text, [FromQuery] string target)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(target))
        {
            return BadRequest("Text and target language must be provided.");
        }

        try
        {
            var translationResult = _translationService.Translate(text, target);
            return Ok(translationResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while translating");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}