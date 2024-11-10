using KanaTomo.Models;
using Microsoft.AspNetCore.Mvc;
namespace KanaTomo.Controllers;
using System.Collections.Generic;


[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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
            new TranslationModel
            {
                OriginalText = text,
                TranslatedText = target == "Japanese" ? "こんにちは" : "Hello",
                TargetLanguage = target
            }
        };
        return Ok(translationResult);
    }
}