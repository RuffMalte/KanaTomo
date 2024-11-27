using KanaTomo.Models.Translation;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.API.APITranslation;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class ApiTranslateController : ControllerBase
{
    private readonly IApiTranslationService _translationService;
    private readonly ILogger<ApiTranslateController> _logger;

    public ApiTranslateController(IApiTranslationService translationService, ILogger<ApiTranslateController> logger)
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
    public async Task<ActionResult<TranslationModel>> Translate([FromQuery] string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return BadRequest("Text and target language must be provided.");
        }

        try
        {
            var translationResult = await _translationService.Translate(text);
            return Ok(translationResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while translating");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}