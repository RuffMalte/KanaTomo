using KanaTomo.Models;
using Microsoft.AspNetCore.Mvc;

namespace KanaTomo.API;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class TranslateController : ControllerBase
{
    [HttpGet("helloworld")]
    public ActionResult<string> HelloWorld()
    {
        return "Hello World";
    }

    [HttpGet("translate")]
    public ActionResult<List<TranslationModel>> Translate([FromQuery] string text, [FromQuery] string target)
    {
        // Mock translation logic for demo purposes
        var translationResult = new List<TranslationModel>
        {
            new TranslationModel(text, target == "Japanese" ? "こんにちは" : "Hello", target),
            new TranslationModel("World", target == "Japanese" ? "世界" : "World", target),
            new TranslationModel("Goodbye", target == "Japanese" ? "さようなら" : "Goodbye", target)
        };
        return Ok(translationResult);
    }
}