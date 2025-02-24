using System.Net.Http.Json;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LazyRisparmioforo.Infrastructure.Data;

public class ApplicationDbContextInitializer(
    HttpClient httpClient,
    ApplicationDbContext context,
    ILoggerFactory logger)
{
    private readonly ILogger _logger = logger.CreateLogger<ApplicationDbContextInitializer>();

    public async Task InitializeAsync()
    {
        try
        {
            // await context.Database.EnsureDeletedAsync();
            // await context.Database.EnsureCreatedAsync();
            // await context.Database.MigrateAsync();
            // await SeedCategoriesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Migration error {exception}", exception);
            throw;
        }
    }
    
    private async Task SeedCategoriesAsync()
    {
        var categories = CategoryConstants.IdToLabel
            .Select(kvp => new Category
            {
                Id = kvp.Key,
                Name = kvp.Value
            })
            .ToArray();
        
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
    }
}