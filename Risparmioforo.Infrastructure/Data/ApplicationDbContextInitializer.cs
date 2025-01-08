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
            // await SeedDataAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Migration error {exception}", exception);
            throw;
        }
    }

    private async Task SeedDataAsync()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                ValueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                Description = "Test transaction",
                Amount = 100
            },
            new Transaction
            {
                Id = 2,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                ValueDate = DateOnly.FromDateTime(DateTime.Now),
                Description = "Test transaction 2",
                Amount = 200
            }
        };
        
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }
}