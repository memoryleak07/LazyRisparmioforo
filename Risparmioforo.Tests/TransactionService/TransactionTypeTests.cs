using Microsoft.Extensions.DependencyInjection;
using Risparmioforo.Services.TransactionService;

namespace Risparmioforo.Tests.TransactionService;

public class TransactionServiceTests
{
    private readonly ITransactionService _transactionService;

    public TransactionServiceTests()
    {
        var serviceProvider = new ServiceCollection().AddRequiredServices().BuildServiceProvider();
        _transactionService = serviceProvider.GetRequiredService<ITransactionService>();
    }
    
    [Fact]
    public async Task RemoveTransactionCommand_ReturnsNotSuccess()
    {
        var command = new RemoveTransactionCommand { Id = 1000 };
        var result = await _transactionService.Remove(command, CancellationToken.None);
        
        Assert.False(result.IsSuccess);
    }
    
    
}