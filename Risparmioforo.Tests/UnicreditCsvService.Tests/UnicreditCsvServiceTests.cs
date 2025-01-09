using Risparmioforo.Domain.Transaction;
using Risparmioforo.Services.UnicreditCsvService;

namespace Risparmioforo.Tests.UnicreditCsvService;

public class UnicreditCsvServiceTests
{
    private readonly IUnicreditCsvService _unicreditCsvService;
    private readonly StreamReader _streamReader;

    public UnicreditCsvServiceTests()
    {
        var serviceProvider = new ServiceCollection().AddRequiredServices().BuildServiceProvider();
        _unicreditCsvService = serviceProvider.GetRequiredService<IUnicreditCsvService>();
        _streamReader = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti.csv"));
    }
    
    [Fact]
    public async Task CsvReaderNoError_ReturnsNull()
    {
        var result = await _unicreditCsvService.ReadCsvAsync(_streamReader, CancellationToken.None);
        Assert.Null(result.Error);
    }
    
    [Fact]
    public async Task CsvReaderBadFile_ReturnsNotNull()
    {
        var badFile = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti_BAD.csv"));
        var result = await _unicreditCsvService.ReadCsvAsync(badFile, CancellationToken.None);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task CsvReaderOnlyPayments_ReturnsFalse()
    {
        var onlyPaymentsFile = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti_OnlyPayments.csv"));
        var result = await _unicreditCsvService.ReadCsvAsync(onlyPaymentsFile, CancellationToken.None);
        if (result.Value == null)
        {
            Assert.Fail();
            return;
        }

        var anyOperationNotPayment = result.Value.ToList().Any(x => x.Operation != TransactionOperation.Payment);
        Assert.False(anyOperationNotPayment);
    }
    
    [Fact]
    public async Task CsvReaderOnlyPayments_ReturnsFourteenItemsFound()
    {
        // TODO: there is a payment with posive amount, fix regex
        var onlyPaymentsFile = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti_OnlyPayments.csv"));
        var result = await _unicreditCsvService.ReadCsvAsync(onlyPaymentsFile, CancellationToken.None);
        if (result.Value == null)
        {
            Assert.Fail();
            return;
        }

        var count = result.Value.Count(x => x.Operation == TransactionOperation.Payment);
        Assert.Equal(14, count);
    }
    
    [Fact]
    public async Task CsvReaderOnlyPayments_ReturnsSevenWithdrawalFound()
    {
        var fileStream = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti_SevenWithdrawal.csv"));
        var result = await _unicreditCsvService.ReadCsvAsync(fileStream, CancellationToken.None);
        if (result.Value == null)
        {
            Assert.Fail();
            return;
        }

        var count = result.Value.Count(x => x.Operation == TransactionOperation.Withdraw);
        Assert.Equal(7, count);
    }
    
    [Fact]
    public async Task CsvReaderAll_ReturnsSevenDebitsFound()
    {
        var fileStream = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti_SevenWithdrawal.csv"));
        var result = await _unicreditCsvService.ReadCsvAsync(fileStream, CancellationToken.None);
        if (result.Value == null)
        {
            Assert.Fail();
            return;
        }

        var count = result.Value.Count(x => x.Operation == TransactionOperation.Debit);
        Assert.Equal(7, count);
    }
    
    [Fact]
    public async Task CsvReaderAll_ReturnsThreeFeesFound()
    {
        var fileStream = new StreamReader(File.OpenRead(@"C:\Users\Marco\Downloads\Elenco_Movimenti_SevenWithdrawal.csv"));
        var result = await _unicreditCsvService.ReadCsvAsync(fileStream, CancellationToken.None);
        if (result.Value == null)
        {
            Assert.Fail();
            return;
        }

        var count = result.Value.Count(x => x.Operation == TransactionOperation.Fee);
        Assert.Equal(3, count);
    }
}