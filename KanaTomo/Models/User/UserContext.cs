using Microsoft.EntityFrameworkCore;

namespace KanaTomo.Models.User;

public class UserContext: DbContext
{
    public DbSet<UserModel> Users { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options): base(options)
    {
        
    }
}