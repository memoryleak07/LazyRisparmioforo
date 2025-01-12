using Risparmioforo.Domain.Account;
using Risparmioforo.Services.CategoryService;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Services.AccountService;

public interface IAccountService
{
    Task<Result<Pagination<AccountDto>>> Search(SearchCommand command, CancellationToken cancellationToken);
    Task<Result<AccountDto>> Create(CreateAccountCommand command, CancellationToken cancellationToken);
    Task<Result<AccountDto>> Update(UpdateAccountCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> Remove(RemoveAccountCommand command, CancellationToken cancellationToken);
}