using Risparmioforo.Domain.Account;
using Risparmioforo.Domain.Category;
using Risparmioforo.Domain.Common;
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
        await SeedAccounts();
        await SeedCategories();
        await SeedTransactions();
    }
    
    private async Task SeedAccounts()
    {
        var accounts = new List<Account>()
        {
            new() { Id = (int)AccountDefault.Main, Name = "Main", Amount = 10_000 },
            new() { Id = (int)AccountDefault.Cash, Name = "Cash", Amount = 100 }
        };
        context.Accounts.AddRange(accounts);
        await context.SaveChangesAsync();
    }
    private async Task SeedCategories()
    {
        var categories = new List<Category>()
        {
            // From azure document intelligence
            // https://learn.microsoft.com/en-us/azure/ai-services/document-intelligence/prebuilt/receipt?view=doc-intel-4.0.0
            new() { Flow = Flow.Expense, Name = "Meal", Keywords = ["bar", "restaurant", "coffee", "caffè", "chalet"]},
            new() { Flow = Flow.Expense, Name = "Supplies" },
            new() { Flow = Flow.Expense, Name = "Hotel", Keywords = ["hotel", "b&b"] },
            new() { Flow = Flow.Expense, Name = "Fuel&Energy", Keywords = ["benzina", "gas", "distributore", "petrol", "carburant"] },
            new() { Flow = Flow.Expense, Name = "Transportation", Keywords = ["park", "parcheggio"]},
            new() { Flow = Flow.Expense, Name = "Communication" },
            new() { Flow = Flow.Expense, Name = "Subscriptions", Keywords = ["amazon prime", "github", "netflix", "spotify", "youtube"] },
            new() { Flow = Flow.Expense, Name = "Training", Keywords = ["gym", "palestra", "fitness"] },
            new() { Flow = Flow.Expense, Name = "Healthcare", Keywords = ["dott.", "medico", "dottore", "farmacia"] },
            // custom
            new() { Flow = Flow.Expense, Name = "Insurance", Keywords = ["unipolsai", "assicurazione"] },
            new() { Flow = Flow.Expense, Name = "Fines", Keywords = ["pagamenti p.a."] },
            new() { Flow = Flow.Expense, Name = "Shopping", Keywords = ["amazon", "amzn"] },
            new() { Flow = Flow.Expense, Name = "Clothing", Keywords = ["zalando"] },
            new() { Flow = Flow.Expense, Name = "Telephone", Keywords = ["vodafone", "ho. mobile"] },
            new() { Flow = Flow.Income, Name = "Salary", Keywords = ["alten"] },
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
    }
    private async Task SeedTransactions()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                ValueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                Description = "Test income",
                Amount = 10_000,
                Flow = Flow.Income,
                Operation = TransactionOperation.Transfer,
                AccountId = (int)AccountDefault.Main,
            },
            new Transaction
            {
                Id = 2,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                ValueDate = DateOnly.FromDateTime(DateTime.Now),
                Description = "Test withdraw",
                Amount = -1_000,
                Flow = Flow.Expense,
                Operation = TransactionOperation.Withdraw,
                AccountId = (int)AccountDefault.Cash,
            },
            new Transaction
            {
                Id = 3,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                ValueDate = DateOnly.FromDateTime(DateTime.Now),
                Description = "Test cash",
                Amount = -100,
                Flow = Flow.Expense,
                Operation = TransactionOperation.Payment,
                AccountId = (int)AccountDefault.Cash
            }
        };

        context.Transactions.AddRange(transactions);
        await context.SaveChangesAsync();
    }
}