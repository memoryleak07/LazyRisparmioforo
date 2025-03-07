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
                .IsInEnum().WithMessage("Enter a valid flow.");
        }
    }
    
    public class TransactionCreateValidator : AbstractValidator<TransactionCreateCommand>
    {
        public TransactionCreateValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull().NotEmpty().WithMessage("Amount is not valid.");

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Category is required.");
            
            RuleFor(x => x.Description)
                .MaximumLength(4000).WithMessage("Description maximum length is 4000.");
        }
    }
    
    public class TransactionUpdateValidator : AbstractValidator<TransactionUpdateCommand>
    {
        public TransactionUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Amount)
                .NotNull().NotEmpty().WithMessage("Amount is not valid.");
            
            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Category is required.");
            
            RuleFor(x => x.Description)
                .MaximumLength(4000).WithMessage("Description maximum length is 4000.");
        }
    }
    
    public class TransactionRemoveValidator : AbstractValidator<TransactionRemoveCommand>
    {
        public TransactionRemoveValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().NotEmpty().WithMessage("Id is required.");
        }
    }
}