using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Services.TransactionService;

public interface ITransactionService
{
    Task<Result<Pagination<TransactionDto>>> Search(SearchCommand command, CancellationToken cancellationToken);
    Task<Result<TransactionDto>> Create(CreateTransactionCommand command, CancellationToken cancellationToken);
    Task<Result<TransactionDto>> Update(UpdateTransactionCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> Remove(RemoveTransactionCommand command, CancellationToken cancellationToken);
}