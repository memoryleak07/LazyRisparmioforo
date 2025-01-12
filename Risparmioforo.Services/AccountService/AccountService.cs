using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Account;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Services.AccountService;

public class AccountService(
    ApplicationDbContext dbContext,
    ILogger<AccountService> logger,
    IAccountValidator accountValidator)
     : IAccountService
{
    public async Task<Result<Pagination<AccountDto>>> Search(SearchCommand command, CancellationToken cancellationToken)
    {
        var query = dbContext.Accounts
            .AsNoTracking()
            .Where(string.IsNullOrEmpty(command.Query)
                ? account => true
                : account => account.Name.ToLower() == command.Query.ToLower())
            .AsQueryable();

        var totalItemsCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(command.PageSize * command.PageIndex)
            .Take(command.PageSize)
            .ToListAsync(cancellationToken);

        var dto = items.ToDto();
        var paginatedResults = dto.ToPagination(command.PageIndex, command.PageSize, totalItemsCount);

        return Result<Pagination<AccountDto>>.Success(paginatedResults);
    }
    
    public async Task<Result<AccountDto>> Create(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var entity = new Account { Name = command.Name, Amount = command.InitialAmount };
        
        var validationResult = await accountValidator.ValidateAsync(entity, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountDto>.Failure(AccountErrors.ValidationErrors(errors));
        }
        
        if (await IsNameNotUnique(command.Name))
            return Result<AccountDto>.Failure(AccountErrors.NotUnique);

        dbContext.Accounts.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Account created successfully.");
        
        return Result<AccountDto>.Success(entity.ToDto());
    }
    
    public async Task<Result<AccountDto>> Update(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        if (await IsNameNotUnique(command.Name, command.Id))
            return Result<AccountDto>.Failure(AccountErrors.NotUnique);

        var entity = await dbContext.Accounts
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<AccountDto>.Failure(AccountErrors.NotFound(command.Id));

        entity.Name = command.Name;
        entity.Amount = command.Amount;
        
        var validationResult = await accountValidator.ValidateAsync(entity, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountDto>.Failure(AccountErrors.ValidationErrors(errors));
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Account updated successfully.");
        
        return Result<AccountDto>.Success(entity.ToDto());
    }
    
    public async Task<Result<bool>> Remove(RemoveAccountCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Accounts
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<bool>.Failure(AccountErrors.NotFound(command.Id));
        
        dbContext.Accounts.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Account removed successfully.");

        return Result<bool>.Success(true);
    }
    
    private async Task<bool> IsNameNotUnique(string name, int? id = null) => 
        await dbContext.Accounts.AnyAsync(x => x.Name.ToLower() == name.ToLower() && (id == null || x.Id != id));
}