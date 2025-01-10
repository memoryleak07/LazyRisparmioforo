using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public interface IDocumentIntelligenceService
{
    Task<Result<ICollection<Transaction>>> ReadReceiptDocumentsAsync(byte[] bytes, CancellationToken cancellationToken);
}