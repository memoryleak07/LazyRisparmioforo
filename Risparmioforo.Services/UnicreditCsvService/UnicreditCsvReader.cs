using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.UnicreditCsvService;

public static class UnicreditCsvReader
{
    private static TransactionMerchant? ExtractMerchantFromText(string? description, TransactionCategory category)
    {
        if (string.IsNullOrEmpty(description) 
            || description.Length == 0
            || category is not TransactionCategory.Payment)
            return null;
        
        int length = description.Length;
        
        _ = description.Substring(0, Math.Min(50, length));
        string group2 = length > 50 ? description.Substring(50, Math.Min(50, length - 50)) : "";
        string group3 = length > 100 ? description.Substring(100, Math.Min(50, length - 100)) : "";
        string group4 = length > 150 ? description.Substring(150, Math.Min(50, length - 150)) : "";
        string group5 = length > 200 ? description[200..] : "";

        return new TransactionMerchant
        {
            MerchantType = DetermineMerchantType(group2),
            CardNumber = DetermineCardNumber(group3),
            Name = group4.Trim(),
            Location = group5.Trim(),
        };
    }

    private static TransactionCategory DetermineCategory(string? description, TransactionType type)
    {
        if (string.IsNullOrEmpty(description))
            return TransactionCategory.Undefined;

        var relevantPatterns = type switch
        {
            TransactionType.Income => UnicreditRegexes.IncomeCategories,
            TransactionType.Expense => UnicreditRegexes.ExpenseCategories,
            _ => []
        };

        foreach (var category in relevantPatterns)
        {
            if (UnicreditRegexes.CategoryPatterns[category].IsMatch(description))
                return category;
        }

        return TransactionCategory.Undefined;
    }

    private static MerchantType DetermineMerchantType(string substring)
    {
        if (UnicreditRegexes.MerchantEcommercePattern.IsMatch(substring))
            return MerchantType.ECommerce;

        if (UnicreditRegexes.MerchantContactlessPattern.IsMatch(substring))
            return MerchantType.Store;

        return MerchantType.Undefined;
    }

    private static string DetermineCardNumber(string substring)
    {
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
    
    public static Transaction ToTransaction(this UnicreditCsvModel csvDto)
    {
        TransactionType type = DetermineTransactionType(csvDto.Importo);
        TransactionCategory category = DetermineCategory(csvDto.Descrizione, type);
        TransactionMerchant? merchant = ExtractMerchantFromText(csvDto.Descrizione, category);

        return new Transaction
        {
            RegistrationDate = csvDto.DataRegistraz,
            ValueDate = csvDto.DataValuta,
            Description = csvDto.Descrizione?.Trim() ?? "",
            Amount = csvDto.Importo,
            Type = type,
            Category = category,
            Merchant = merchant,
        };
    }

    public static ICollection<Transaction> ToTransactions(this ICollection<UnicreditCsvModel> collection)
    {
        return collection.Select(x => x.ToTransaction()).ToList();
    }
}