using FluentValidation;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;

namespace LazyRisparmioforo.Domain.Validators;

public class TransactionValidators
{
    public class TransactionSearchValidator : AbstractValidator<TransactionSearchCommand>
    {
        public TransactionSearchValidator()
        {
            RuleFor(x => x.Flow)
                .IsInEnum().WithMessage("Enter a valid flow");
        }
    }
    
    public class TransactionCreateValidator : AbstractValidator<TransactionCreateCommand>
    {
        public TransactionCreateValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount cannot be null.")
                .NotEmpty().WithMessage("Amount cannot be empty.");
        
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Description cannot be null.")
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(255).WithMessage("Description maximum length is 255.");
        }
    }
    
    public class TransactionUpdateValidator : AbstractValidator<TransactionUpdateCommand>
    {
        public TransactionUpdateValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount cannot be null.")
                .NotEmpty().WithMessage("Amount cannot be empty.");
        
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Description cannot be null.")
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(255).WithMessage("Description maximum length is 255.");
        }
    }
    
    public class TransactionRemoveValidator : AbstractValidator<TransactionRemoveCommand>
    {
        public TransactionRemoveValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id cannot be null.");
        }
    }
}