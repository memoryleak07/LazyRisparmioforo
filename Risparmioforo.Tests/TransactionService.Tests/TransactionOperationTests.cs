namespace Risparmioforo.Tests.TransactionService.Tests;

public class TransactionOperationTests
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionOperationTests()
    {
        var serviceProvider = new ServiceCollection().AddRequiredServices().BuildServiceProvider();
        _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
    // public enum TransactionOperation
    // {
    //     Salary, // TODO: is a Transfer?
    //     Withdraw,
    //     Payment,
    //     Fee,
    //     Transfer,
    //     Debit,
    //     Credit,
    //     Undefined = 99
    // }
    [Fact]
    public async Task AnyTransactionTypeUndefinedWithAmountOk_ReturnsFalse()
    {
        // var items = await _dbContext.Transactions.AsNoTracking().ToListAsync();
        // var incomes = items.Where(x => x.Amount > 0).ToList();
        // var expenses = items.Where(x => x.Amount < 0).ToList();
        //
        // bool anyUndefinedIncome = incomes.Any(x => x.Flow == Flow.Undefined);
        // bool anyUndefinedExpense = expenses.Any(x => x.Flow == Flow.Undefined);
        //
        // Assert.False(anyUndefinedIncome);
        // Assert.False(anyUndefinedExpense);
    }
}