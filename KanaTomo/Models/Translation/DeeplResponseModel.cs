using Newtonsoft.Json;

namespace KanaTomo.Models.Translation;

public class DeeplResponseModel
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("detected_source_language")]
    public string DetectedSourceLanguage { get; set; }

    [JsonProperty("billed_characters")]
    public int BilledCharacters { get; set; }

    [JsonProperty("model_type_used")]
    public string ModelTypeUsed { get; set; }
}