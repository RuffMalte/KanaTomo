using KanaTomo.Models.User;
using Microsoft.EntityFrameworkCore;

namespace KanaTomo.Helper;

public class InitDatabase
{
    public async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
            throw;
        }
    }   
}