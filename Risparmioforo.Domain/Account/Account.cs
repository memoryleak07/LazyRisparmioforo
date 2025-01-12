namespace Risparmioforo.Domain.Account;

public enum AccountDefault
{
    Main = 1,
    Cash = 2,
}
public class Account
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Amount { get; set; }
}