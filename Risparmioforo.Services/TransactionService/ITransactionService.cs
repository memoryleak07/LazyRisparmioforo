using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Services.TransactionService;

public interface ITransactionService
{
    Task<Result<Pagination<Transaction>>> Search(SearchTransactionCommand command, CancellationToken cancellationToken);
    Task<Result<Transaction>> Create(CreateTransactionCommand command, CancellationToken cancellationToken);
    Task<Result<Transaction>> Update(UpdateTransactionCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> Remove(RemoveTransactionCommand command, CancellationToken cancellationToken);
}