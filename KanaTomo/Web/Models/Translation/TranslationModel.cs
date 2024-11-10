using System.ComponentModel.DataAnnotations;

namespace KanaTomo.Web.Models.Translation;

public class TranslationModel
{

    public TranslationModel(string originalText, string translatedText, string targetLanguage)
    {
        OriginalText = originalText;
        TranslatedText = translatedText;
        TargetLanguage = targetLanguage;
        Id = Guid.NewGuid();
    }
    
    [Required]
    public Guid Id { get; set; }
    public string OriginalText { get; set; }
    public string TranslatedText { get; set; }
    public string TargetLanguage { get; set; }
}