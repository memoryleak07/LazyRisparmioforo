using Risparmioforo.Shared.Base;

namespace Risparmioforo.Domain.Account;

public abstract class AccountErrors
{
    public static Error NotFound(int id) => new(
        $"The account with ID = '{id}' was not found.",
        "Account.NotFound");
    
    public static Error NotUnique => new(
        "The account is not unique.",
        "Account.NotUnique");
    
    public static Error ValidationErrors(IEnumerable<string> validationErrors) => new(
        string.Join(", ", validationErrors),
        "Account.ValidationErrors");
}
