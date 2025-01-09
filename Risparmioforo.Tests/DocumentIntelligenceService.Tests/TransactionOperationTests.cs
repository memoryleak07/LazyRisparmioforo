namespace Risparmioforo.Tests.DocumentIntelligenceService.Tests;

public class DocumentIntelligenceServiceTests
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentIntelligenceServiceTests()
    {
        var serviceProvider = new ServiceCollection().AddRequiredServices().BuildServiceProvider();
        _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AnyTransactionTypeUndefinedWithAmountOk_ReturnsFalse()
    {
        // var items = await _dbContext.Transactions.AsNoTracking().ToListAsync();
        // var incomes = items.Where(x => x.Amount > 0).ToList();
        // var expenses = items.Where(x => x.Amount < 0).ToList();
        //
        // bool anyUndefinedIncome = incomes.Any(x => x.Type == TransactionType.Undefined);
        // bool anyUndefinedExpense = expenses.Any(x => x.Type == TransactionType.Undefined);
        //
        // Assert.False(anyUndefinedIncome);
        // Assert.False(anyUndefinedExpense);
    }
}