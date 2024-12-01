using System.ComponentModel.DataAnnotations.Schema;
using KanaTomo.Models.User;

namespace KanaTomo.Models.Anki;

public class AnkiModel
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastReviewDate { get; set; } = DateTime.UtcNow;
    public int ReviewCount { get; set; } = 0;

    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public UserModel? User { get; set; }
}