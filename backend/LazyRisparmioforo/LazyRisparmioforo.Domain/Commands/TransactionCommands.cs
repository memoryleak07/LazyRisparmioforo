using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;

namespace LazyRisparmioforo.Domain.Commands;

public record TransactionGetCommand(
    int Id);

public record TransactionCreateCommand(
    int CategoryId,
    DateOnly Date,
    decimal Amount,
    string? Description);

public record TransactionUpdateCommand(
    int Id,
    int CategoryId,
    DateOnly Date,
    decimal Amount,
    string? Description);

public record TransactionRemoveCommand(
    int Id);

public static class TransactionCommandExtensions
{
    public static Transaction ToEntity(this TransactionCreateCommand command)
    {
        return new Transaction
        {
            Flow = DetermineFlow(command.Amount),
            CategoryId = command.CategoryId,
            RegistrationDate = command.Date,
            ValueDate = command.Date,
            Amount = command.Amount,
            Description = command.Description ?? string.Empty
        };
    }

    public static Transaction ToEntity(this TransactionUpdateCommand command, Transaction existingEntity)
    {
        existingEntity.Flow = DetermineFlow(command.Amount);
        existingEntity.CategoryId = command.CategoryId;
        existingEntity.RegistrationDate = command.Date;
        existingEntity.ValueDate = command.Date;
        existingEntity.Amount = command.Amount;
        existingEntity.Description = command.Description ?? string.Empty;

        return existingEntity;
    }

    private static Flow DetermineFlow(decimal amount) => amount switch
    {
        < 0 => Flow.Expense,
        > 0 => Flow.Income,
        _ => Flow.Undefined
    };
}