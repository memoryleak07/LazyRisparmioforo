namespace Risparmioforo.Domain.Transaction;

public enum TransactionOperation
{
    Withdraw,
    Payment,
    Fee,
    Transfer,
    Debit,
    Credit,
    Undefined = 99
}