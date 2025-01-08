namespace Risparmioforo.Domain.Transaction;

public enum TransactionCategory
{
    Salary, // TODO: is a Transfer?
    Withdraw,
    Payment,
    Fee,
    Transfer,
    Debit,
    Credit,
    Undefined = 99
}