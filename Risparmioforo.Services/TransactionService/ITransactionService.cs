using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Services.TransactionService;

public interface ITransactionService
{
    Task<Result<Pagination<Transaction>>> Search(string? query, int pageIndex = 0, int pageSize = 10);
    Task<Result<Transaction>> Create(CreateTransactionCommand request, CancellationToken cancellationToken);
    Task<Result<Transaction>> Update(UpdateTransactionCommand request, CancellationToken cancellationToken);
    Task<Result<bool>> Remove(RemoveTransactionCommand request, CancellationToken cancellationToken);
}