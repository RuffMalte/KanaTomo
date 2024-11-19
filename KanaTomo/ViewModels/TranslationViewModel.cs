using System.ComponentModel.DataAnnotations;
using KanaTomo.Models.Translation;
using KanaTomo.Models;

namespace KanaTomo.ViewModels;

public class TranslationViewModel
{
    [Required(ErrorMessage = "Please enter text to translate.")]
    public string TextToTranslate { get; set; }
    
    public TranslationModel? TranslationResult { get; set; }
}
