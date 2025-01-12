using FluentValidation;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.TransactionService;

public interface ITransactionValidator : IValidator<Transaction> { }

public class TransactionValidator : AbstractValidator<Transaction>, ITransactionValidator
{
    public TransactionValidator()
    {
        // RuleFor(x => x.Name)
        //     .NotNull().WithMessage("Category name cannot be null.")
        //     .NotEmpty().WithMessage("Category name cannot be empty.")
        //     .MaximumLength(255).WithMessage("Category name maximum length is 255.");
        //
        // RuleFor(x => x.Flow)
        //     .NotNull().WithMessage("Flow cannot be null.")
        //     .IsInEnum().WithMessage("Flow is not valid.");
    }
}
