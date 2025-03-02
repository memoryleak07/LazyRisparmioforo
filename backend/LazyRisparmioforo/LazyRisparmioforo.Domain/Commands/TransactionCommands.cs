using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;

namespace LazyRisparmioforo.Domain.Commands;

public class TransactionSearchCommand : PagedSearchCommand
{
    public Flow? Flow { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}

public record TransactionGetCommand(
    int Id);

public record TransactionCreateCommand(
    int CategoryId,
    DateOnly Date,
    string Description,
    decimal Amount);

public record TransactionUpdateCommand(
    int Id,
    int CategoryId,
    DateOnly Date,
    string Description,
    decimal Amount);

public record TransactionRemoveCommand(
    int Id);

public static class TransactionCommandExtensions
{
    public static Transaction ToEntity(this TransactionCreateCommand command)
    {
        return new Transaction
        {
            CategoryId = command.CategoryId,
            RegistrationDate = command.Date,
            ValueDate = command.Date,
            Description = command.Description,
            Amount = command.Amount,
            Flow = DetermineFlow(command.Amount)
        };
    }

    public static Transaction ToEntity(this TransactionUpdateCommand command, Transaction existingEntity)
    {
        existingEntity.CategoryId = command.CategoryId;
        existingEntity.RegistrationDate = command.Date;
        existingEntity.ValueDate = command.Date;
        existingEntity.Description = command.Description;
        existingEntity.Amount = command.Amount;
        existingEntity.Flow = DetermineFlow(command.Amount);

        return existingEntity;
    }

    private static Flow DetermineFlow(decimal amount) => amount switch
    {
        < 0 => Flow.Expense,
        > 0 => Flow.Income,
        _ => Flow.Undefined
    };
}