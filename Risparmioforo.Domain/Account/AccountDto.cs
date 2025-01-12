namespace Risparmioforo.Domain.Account;

public class AccountDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Amount { get; set; }
}

public static class CategoryMappingExtension
{
    public static ICollection<AccountDto> ToDto(this ICollection<Account> accounts) =>
        accounts.Select(t => t.ToDto()).ToList();

    public static AccountDto ToDto(this Account account) => 
        new()
        {
            Id = account.Id,
            Name = account.Name,
            Amount = account.Amount,
        };
}