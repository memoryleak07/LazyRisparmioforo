using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Api.Services.TransactionService;

public static class TransactionMappings
{
    public static Transaction ToEntity(this CreateTransactionCommand command)
    {
        return new Transaction
        {
            Date = command.Date,
            Description = command.Description,
            Amount = command.Amount
        };
    }

    public static Transaction ToEntity(this UpdateTransactionCommand command, Transaction existingEntity)
    {
        existingEntity.Date = command.Date;
        existingEntity.Description = command.Description;
        existingEntity.Amount = command.Amount;
        return existingEntity;
    }
}