using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.ImportFileService;

public interface IImportFileService
{
    Task<Result<Pagination<TransactionDto>>> ImportCsvAsync(ImportFileCommand command, CancellationToken cancellationToken);
    Task<Result<Pagination<TransactionDto>>> ImportReceiptDocumentsAsync(ImportFileCommand request, CancellationToken cancellationToken);
}