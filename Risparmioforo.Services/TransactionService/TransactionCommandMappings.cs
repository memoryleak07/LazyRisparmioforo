using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.TransactionService;

public static class TransactionCommandMappings
{
    public static Transaction ToEntity(this CreateTransactionCommand command)
    {
        return new Transaction
        {
            RegistrationDate = command.Date,
            ValueDate = command.Date,
            Description = command.Description,
            Amount = command.Amount
        };
    }

    public static Transaction ToEntity(this UpdateTransactionCommand command, Transaction existingEntity)
    {
        existingEntity.RegistrationDate = command.Date;
        existingEntity.ValueDate = command.Date;
        existingEntity.Description = command.Description;
        existingEntity.Amount = command.Amount;
        
        return existingEntity;
    }
}