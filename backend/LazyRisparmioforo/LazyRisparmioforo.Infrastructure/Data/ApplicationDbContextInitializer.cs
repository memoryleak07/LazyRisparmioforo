using LazyRisparmioforo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LazyRisparmioforo.Infrastructure.Data;

public class ApplicationDbContextInitializer(
    ApplicationDbContext context,
    IConfiguration configuration,
    ILoggerFactory logger)
{
    private readonly ILogger _logger = logger.CreateLogger<ApplicationDbContextInitializer>();

    public async Task InitializeAsync()
    {
        try
        {
            // await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
            if (!context.Categories.Any())
                await SeedCategoriesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Migration error {exception}", exception);
            throw;
        }
    }
    
    private async Task SeedCategoriesAsync()
    {
        var categories = configuration.GetSection("Categories").Get<ICollection<Category>>();
        if (categories is null) 
            throw new ArgumentNullException(nameof(categories));
        
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
    }
}