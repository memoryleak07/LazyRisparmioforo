using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Tests.TransactionService;

public class TransactionTypeTests
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionTypeTests()
    {
        var serviceProvider = new ServiceCollection().AddRequiredServices().BuildServiceProvider();
        _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
    
    [Fact]
    public async Task AnyTransactionTypeUndefinedWithAmountOk_ReturnsFalse()
    {
        var items = await _dbContext.Transactions.AsNoTracking().ToListAsync();
        var incomes = items.Where(x => x.Amount > 0).ToList();
        var expenses = items.Where(x => x.Amount < 0).ToList();

        bool anyUndefinedIncome = incomes.Any(x => x.Type == TransactionType.Undefined);
        bool anyUndefinedExpense = expenses.Any(x => x.Type == TransactionType.Undefined);
        
        Assert.False(anyUndefinedIncome);
        Assert.False(anyUndefinedExpense);
    }
    
    [Fact]
    public async Task AnyTransactionUndefinedWithAmountNotZero_ReturnsFalse()
    {
        var items = await _dbContext.Transactions.AsNoTracking().ToListAsync();
        var undefinedItems = items.Where(x => x.Type == TransactionType.Undefined).ToList();
        if (undefinedItems.Count == 0)  return;

        bool anyUndefinedAmountNotZero = undefinedItems.Any(x => x.Amount != 0);
        
        Assert.False(anyUndefinedAmountNotZero);
    }
}