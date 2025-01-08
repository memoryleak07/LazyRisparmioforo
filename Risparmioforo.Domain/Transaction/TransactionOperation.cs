namespace Risparmioforo.Domain.Transaction;

public enum TransactionOperation
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