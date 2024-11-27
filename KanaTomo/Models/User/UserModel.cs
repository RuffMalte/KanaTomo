using System.ComponentModel.DataAnnotations.Schema;

namespace KanaTomo.Models.User;

public class UserModel
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
}