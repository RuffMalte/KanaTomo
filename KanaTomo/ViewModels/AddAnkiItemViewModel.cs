using System.ComponentModel.DataAnnotations;

namespace KanaTomo.ViewModels;

public class AddAnkiItemViewModel
{
    [Required(ErrorMessage = "Front side is required")]
    [StringLength(500, ErrorMessage = "Front side cannot be longer than 500 characters")]
    public string Front { get; set; }

    [Required(ErrorMessage = "Back side is required")]
    [StringLength(500, ErrorMessage = "Back side cannot be longer than 500 characters")]
    public string Back { get; set; }
}