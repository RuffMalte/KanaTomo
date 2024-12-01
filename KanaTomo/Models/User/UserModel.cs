using System.ComponentModel.DataAnnotations.Schema;
using KanaTomo.Models.Anki;

namespace KanaTomo.Models.User;

public class UserModel
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;
    
    public string ProfilePictureUrl { get; set; } = string.Empty;
    
    public DateTime LastActivityDate { get; set; } = DateTime.UtcNow;
    public int TotalLogins { get; set; } = 0;
    
    public int XP { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int NextLevelXP { get; set; } = 100;
    
    public float OverallAccuracy { get; set; } = 0f;
    public float LastAccuracy { get; set; } = 0f;
    public int TotalReviews { get; set; } = 0;
    public int TotalCorrect { get; set; } = 0;
    public int TotalIncorrect { get; set; } = 0;
    
    //ANKI
    public List<AnkiModel> AnkiItems { get; set; } = new List<AnkiModel>();

}