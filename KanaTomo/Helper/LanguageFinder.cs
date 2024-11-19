using DeepL;
using KanaTomo.Models.Translation;

namespace KanaTomo.Helper;


public class LanguageFinder
{
    public string? DetectLanguage(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        int japaneseCharCount = 0;
        int englishCharCount = 0;

        foreach (char c in text)
        {
            if (IsJapaneseChar(c))
                japaneseCharCount++;
            else if (IsEnglishChar(c))
                englishCharCount++;
        }

        if (japaneseCharCount > englishCharCount)
            return "ja";
        else if (englishCharCount > japaneseCharCount)
            return "en-US";
        else
            return null;
    }

    private bool IsJapaneseChar(char c)
    {
        return (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
               (c >= 0x30A0 && c <= 0x30FF) ||  // Katakana
               (c >= 0x4E00 && c <= 0x9FFF);    // Kanji
    }

    private bool IsEnglishChar(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    }
}