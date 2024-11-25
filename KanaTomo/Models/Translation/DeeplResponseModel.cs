using System.Text.Json.Serialization;

namespace KanaTomo.Models.Translation;

public class DeeplResponseModel
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("detected_source_language")]
    public string DetectedSourceLanguage { get; set; }

    [JsonPropertyName("billed_characters")]
    public int BilledCharacters { get; set; }

    [JsonPropertyName("model_type_used")]
    public string ModelTypeUsed { get; set; }
}