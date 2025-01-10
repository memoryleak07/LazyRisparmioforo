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

    private static TransactionOperation DetermineOperation(string? description, TransactionType type)
    {
        if (string.IsNullOrEmpty(description))
            return TransactionOperation.Undefined;

        var relevantPatterns = type switch
        {
            TransactionType.Income => UnicreditRegexes.IncomeOperationPatterns,
            TransactionType.Expense => UnicreditRegexes.ExpenseOperationPatterns,
            _ => []
        };

        foreach (var category in relevantPatterns)
        {
            if (UnicreditRegexes.TransactionOperationPatterns[category].IsMatch(description))
                return category;
        }

        return TransactionOperation.Undefined;
    }

    private static TransactionMethod DetermineTransactionMethod(string? description)
    {
        if (string.IsNullOrEmpty(description))
            return TransactionMethod.Undefined;
        
        if (UnicreditRegexes.EcommercePattern.IsMatch(description))
            return TransactionMethod.Card;

        if (UnicreditRegexes.ContactlessPattern.IsMatch(description))
            return TransactionMethod.Card;

        return TransactionMethod.Undefined;
    }

    private static string ExtractCardNumberFromText(string? description)
    {
        if (string.IsNullOrEmpty(description) || description.Length < 100)
            return string.Empty;
        
        var substring = description.Substring(100, Math.Min(50, description.Length - 100));
        var matchCardNumber = UnicreditRegexes.CardNumberPattern.Match(substring);
        return matchCardNumber.Success
            ? matchCardNumber.Groups[1].Value
            : string.Empty;
    }

    private static TransactionType DetermineTransactionType(decimal amount)
    {
        return amount switch
        {
            < 0 => TransactionType.Expense,
            > 0 => TransactionType.Income,
            _ => TransactionType.Undefined
        };
    }
    
    public static Transaction ToTransaction(this UnicreditCsvModel model)
    {
        TransactionType type = DetermineTransactionType(model.Importo);
        TransactionOperation operation = DetermineOperation(model.Descrizione, type);

        return new Transaction
        {
            RegistrationDate = model.DataRegistraz,
            ValueDate = model.DataValuta,
            Description = model.Descrizione?.Trim() ?? "",
            Amount = model.Importo,
            Type = type,
            Operation = operation,
            Method = DetermineTransactionMethod(model.Descrizione),
            Card = ExtractCardNumberFromText(model.Descrizione),
            Merchant = ExtractMerchantFromText(model.Descrizione, operation),
            // Items = [] // TODO: extract items, how?
        };
    }

    public static ICollection<Transaction> ToTransactions(this ICollection<UnicreditCsvModel> collection)
    {
        return collection.Select(x => x.ToTransaction()).ToList();
    }
}