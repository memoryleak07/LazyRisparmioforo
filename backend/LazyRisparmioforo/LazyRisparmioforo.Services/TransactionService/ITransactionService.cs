﻿using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Domain.Queries;
using LazyRisparmioforo.Shared.Shared;

namespace TransactionService;

public interface ITransactionService
{
    Task<Result<Transaction>> GetAsync(TransactionGetCommand command, CancellationToken cancellationToken);
    Task<Pagination<Transaction>> SearchAsync(TransactionSearchQuery query, CancellationToken cancellationToken);
    Task<Result> CreateAsync(TransactionCreateCommand command, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(TransactionUpdateCommand command, CancellationToken cancellationToken);
    Task<Result> RemoveAsync(TransactionRemoveCommand command, CancellationToken cancellationToken);
}