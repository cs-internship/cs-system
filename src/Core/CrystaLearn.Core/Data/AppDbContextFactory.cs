using Microsoft.EntityFrameworkCore.Design;

namespace CrystaLearn.Core.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use a default connection string for migrations
        optionsBuilder.UseNpgsql("Host=localhost;Database=CrystaLearn;Username=postgres;Password=postgres");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
