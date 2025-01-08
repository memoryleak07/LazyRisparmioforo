using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.UnicreditCsvService;

public interface IUnicreditCsvService
{
    Task<Result<ICollection<Transaction>>> ReadCsvAsync(StreamReader streamReader, CancellationToken cancellationToken);
}