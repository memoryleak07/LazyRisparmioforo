using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.ImportFileService;

public interface IImportFileService
{
    Task<Result<bool>> ImportCsvAsync(ImportFileCommand command, CancellationToken cancellationToken);
    Task<Result<ICollection<TransactionDto>>> ImportReceiptDocumentsAsync(ImportFileCommand request, CancellationToken cancellationToken);
}