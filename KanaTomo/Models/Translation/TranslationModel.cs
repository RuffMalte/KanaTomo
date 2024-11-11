using System.ComponentModel.DataAnnotations;

namespace KanaTomo.Models.Translation;

public class TranslationModel
{
    public string OriginalText { get; set; }
    public string TargetLanguage { get; set; }
    public JishoResponse JishoResponse { get; set; }
    
    // Placeholder for future implementations
    // public GoogleResponse GoogleResponse { get; set; }
    // public DeeplResponse DeeplResponse { get; set; }

    public TranslationModel(string originalText, string targetLanguage)
    {
        OriginalText = originalText;
        TargetLanguage = targetLanguage;
    }
}
