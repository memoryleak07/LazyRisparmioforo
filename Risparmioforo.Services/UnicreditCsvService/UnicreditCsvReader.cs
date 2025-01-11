using Risparmioforo.Domain.Common;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.UnicreditCsvService;

public static class UnicreditCsvReader
{
    private static TransactionMerchant? ExtractMerchantFromText(string? description, TransactionOperation operation)
    {
        if (string.IsNullOrEmpty(description) 
            || description.Length == 0
            || operation is not TransactionOperation.Payment)
            return null;
        
        string group4 = description.Length > 150 ? description.Substring(150, Math.Min(50, description.Length - 150)) : "";
        string group5 = description.Length > 200 ? description[200..] : "";
    
        return new TransactionMerchant
        {
            Name = group4.Trim(),
            Location = group5.Trim(),
        };
    }

    private static TransactionOperation DetermineOperation(string? description, Flow type)
    {
        if (string.IsNullOrEmpty(description))
            return TransactionOperation.Undefined;

        var relevantPatterns = type switch
        {
            Flow.Income => UnicreditRegexes.IncomeOperationPatterns,
            Flow.Expense => UnicreditRegexes.ExpenseOperationPatterns,
            _ => []
        };

        foreach (var operation in relevantPatterns)
        {
            if (UnicreditRegexes.TransactionOperationPatterns[operation].IsMatch(description))
                return operation;
        }

        return TransactionOperation.Undefined;
    }

    private static TransactionMethod DetermineTransactionMethod(TransactionOperation transactionOperation)
    {
        return transactionOperation switch
        {
            TransactionOperation.Withdraw => TransactionMethod.Cash,
            TransactionOperation.Transfer 
                or TransactionOperation.Payment
                or TransactionOperation.Credit 
                or TransactionOperation.Debit
                or TransactionOperation.Fee => TransactionMethod.Card,
            _ => TransactionMethod.Undefined
        };
    }

    private static string ExtractCardNumberFromText(string? description)
    {
        if (string.IsNullOrEmpty(description))
            return string.Empty;

        var matchCardNumber = UnicreditRegexes.CardNumberPattern.Match(description);
        if (!matchCardNumber.Success) 
            return string.Empty;

        if (!string.IsNullOrEmpty(matchCardNumber.Groups[1].Value))
            return matchCardNumber.Groups[1].Value;
        
        if (!string.IsNullOrEmpty(matchCardNumber.Groups[2].Value))
            return matchCardNumber.Groups[2].Value;
        
        return string.Empty;
    }

    private static Flow DetermineFlow(decimal amount)
    {
        return amount switch
        {
            < 0 => Flow.Expense,
            > 0 => Flow.Income,
            _ => Flow.Undefined
        };
    }
    
    public static Transaction ToTransaction(this UnicreditCsvModel model)
    {
        Flow flow = DetermineFlow(model.Importo);
        TransactionOperation operation = DetermineOperation(model.Descrizione, flow);

        return new Transaction
        {
            RegistrationDate = model.DataRegistraz,
            ValueDate = model.DataValuta,
            Description = model.Descrizione?.Trim() ?? "",
            Amount = model.Importo,
            Flow = flow,
            Operation = operation,
            CardNumber = ExtractCardNumberFromText(model.Descrizione),
            Method = DetermineTransactionMethod(operation),
            Merchant = ExtractMerchantFromText(model.Descrizione, operation),
            // Items = [] // TODO: extract items, how?
        };
    }

    public static ICollection<Transaction> ToTransactions(this ICollection<UnicreditCsvModel> collection)
    {
        return collection.Select(x => x.ToTransaction()).ToList();
    }
}