using System.ComponentModel.DataAnnotations;
using KanaTomo.Models;

namespace KanaTomo.ViewModels;

public class TranslationViewModel
{
    [Required(ErrorMessage = "Please enter text to translate.")]
    public string TextToTranslate { get; set; }

    [Required(ErrorMessage = "Please select a target language.")]
    public string TargetLanguage { get; set; }

    public List<TranslationModel>? TranslationResults { get; set; } = new List<TranslationModel>();
}