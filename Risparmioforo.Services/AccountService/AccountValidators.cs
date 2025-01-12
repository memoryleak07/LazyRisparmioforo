using FluentValidation;
using Risparmioforo.Domain.Account;

namespace Risparmioforo.Services.AccountService;

public interface IAccountValidator : IValidator<Account> { }

public class AccountValidator : AbstractValidator<Account>, IAccountValidator
{
    public AccountValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Account name cannot be null.")
            .NotEmpty().WithMessage("Category name cannot be empty.")
            .MaximumLength(255).WithMessage("Account name maximum length is 255.");
    }
}
