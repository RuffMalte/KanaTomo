using KanaTomo.Models.Anki;

namespace KanaTomo.ViewModels.Anki;

public class AnkiCardListViewModel
{
    public List<AnkiModel> Cards { get; set; } = new List<AnkiModel>();
    public int DueCardsCount { get; set; }
    public int TotalCardsCount { get; set; }
}