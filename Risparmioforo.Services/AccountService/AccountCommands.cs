namespace Risparmioforo.Services.AccountService;

public class CreateAccountCommand
{
    public string Name { get; set; }
    public decimal InitialAmount { get; set; }
}

public class UpdateAccountCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
}

public class RemoveAccountCommand
{
    public int Id { get; set; }
}