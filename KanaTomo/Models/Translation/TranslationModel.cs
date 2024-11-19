using System.ComponentModel.DataAnnotations;
using DeepL.Model;

namespace KanaTomo.Models.Translation;

using System.Text.Json.Serialization;

public class TranslationModel
{
    [JsonPropertyName("originalText")]
    public string OriginalText { get; set; }

    [JsonPropertyName("targetLanguage")]
    public string TargetLanguage { get; set; }

    [JsonPropertyName("jishoResponse")]
    public JishoResponse JishoResponse { get; set; }

    [JsonPropertyName("deeplResponse")]
    public DeeplResponseModel DeeplResponse { get; set; }

    public TranslationModel(string originalText, string targetLanguage)
    {
        OriginalText = originalText;
        TargetLanguage = targetLanguage;
    }
}