using System.Text.Json.Serialization;

namespace KanaTomo.Models.Translation;

public class JishoResponse
{
    [JsonPropertyName("data")]
    public List<JishoData> Data { get; set; }
}

public class JishoData
{
    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("is_common")]
    public bool IsCommon { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }

    [JsonPropertyName("jlpt")]
    public List<string> Jlpt { get; set; }

    [JsonPropertyName("japanese")]
    public List<JapaneseWord> Japanese { get; set; }

    [JsonPropertyName("senses")]
    public List<Sense> Senses { get; set; }
}

public class JapaneseWord
{
    [JsonPropertyName("word")]
    public string Word { get; set; }

    [JsonPropertyName("reading")]
    public string Reading { get; set; }
}

public class Sense
{
    [JsonPropertyName("english_definitions")]
    public List<string> EnglishDefinitions { get; set; }

    [JsonPropertyName("parts_of_speech")]
    public List<string> PartsOfSpeech { get; set; }
}