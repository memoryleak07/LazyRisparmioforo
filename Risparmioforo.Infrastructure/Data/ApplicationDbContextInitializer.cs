using Risparmioforo.Domain.Category;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Infrastructure.Data;

public class ApplicationDbContextInitializer(
    ApplicationDbContext context,
    ILoggerFactory logger)
{
    private readonly ILogger _logger = logger.CreateLogger<ApplicationDbContextInitializer>();

    public async Task InitializeAsync()
    {
        try
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
            await SeedDataAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Migration error {exception}", exception);
            throw;
        }
    }

    private async Task SeedDataAsync()
    {
        var categories = new List<Category>()
        {
            new() { Name = "Insurance" },
            new() { Name = "Fines" },
            new() { Name = "Transport" },
            new() { Name = "Education" },
            new() { Name = "Health" },
            new() { Name = "Shopping" },
            new() { Name = "Clothing" },
            new() { Name = "Beauty" },
            new() { Name = "Travel" },
            new() { Name = "Grocery" },
            new() { Name = "Bar" },
            new() { Name = "Tobacco" },
            new() { Name = "Gift" },
            new() { Name = "Equipment" },
            new() { Name = "Rent" },
            new() { Name = "Tax" },
            new() { Name = "Saving" },
            new() { Name = "Investment" },
        };
        
        // var transactions = new List<Transaction>
        // {
        //     new Transaction
        //     {
        //         Id = 1,
        //         RegistrationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
        //         ValueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
        //         Description = "Test transaction",
        //         Amount = 100
        //     },
        //     new Transaction
        //     {
        //         Id = 2,
        //         RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
        //         ValueDate = DateOnly.FromDateTime(DateTime.Now),
        //         Description = "Test transaction 2",
        //         Amount = 200
        //     }
        // };
        
        // await context.Transactions.AddRangeAsync(transactions);
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
}