using KanaTomo.Models.Anki;

namespace KanaTomo.ViewModels.Anki;

public class AnkiCardListViewModel
{
    public List<AnkiModel> Cards { get; set; } = new List<AnkiModel>();
}