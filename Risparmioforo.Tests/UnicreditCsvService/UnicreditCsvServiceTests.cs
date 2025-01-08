namespace Risparmioforo.Tests.UnicreditCsvService;

public class UnicreditCsvServiceTests
{
    private readonly ApplicationDbContext _dbContext;

    public UnicreditCsvServiceTests()
    {
        var serviceProvider = new ServiceCollection().AddRequiredServices().BuildServiceProvider();
        _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
    [Fact]
    public void Test1()
    {
    }
}