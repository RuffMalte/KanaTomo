using KanaTomo.Models.Anki;
using Microsoft.EntityFrameworkCore;

namespace KanaTomo.Models.User;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<AnkiModel> AnkiItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>()
            .HasMany(u => u.AnkiItems)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);
    }
}