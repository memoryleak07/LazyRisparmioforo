using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.TransactionService;

public static class TransactionCsvReader
{
    private static TransactionMerchant? ExtractMerchantFromText(string? description, TransactionCategory category)
    {
        if (string.IsNullOrEmpty(description) || category is not TransactionCategory.Payment) 
            return null;
        
        string substring = description.Substring(150, description.Length - 150);

        return new TransactionMerchant
        {
            Name = substring[..50].Trim(),
            Location = substring[50..].Trim()
        };
    }
    
    private static TransactionCategory DetermineCategory(string? description, TransactionType type)
    {
        if (string.IsNullOrEmpty(description)) 
            return TransactionCategory.Undefined; 
        
        var relevantPatterns = type switch
        {
            TransactionType.Income => TransactionRegexes.IncomeCategories,
            TransactionType.Expense => TransactionRegexes.ExpenseCategories,
            _ => []
        };
        
        foreach (var category in relevantPatterns)
        {
            if (TransactionRegexes.CategoryPatterns[category].IsMatch(description))
                return category;
        }
        return TransactionCategory.Undefined;
    }

    private static TransactionType DetermineType(decimal amount)
    {
        return amount switch
        {
            < 0 => TransactionType.Expense,
            > 0 => TransactionType.Income,
            _ => TransactionType.None
        };
    }

    
    public static Transaction ToTransaction(this TransactionCsvModel csvDto)
    {
        TransactionType type = DetermineType(csvDto.Importo);
        TransactionCategory category = DetermineCategory(csvDto.Descrizione, type);
        TransactionMerchant? merchant = ExtractMerchantFromText(csvDto.Descrizione, category);
        
        return new Transaction
        {
            RegistrationDate = csvDto.DataRegistrazione,
            ValueDate = csvDto.DataValuta,
            Description = csvDto.Descrizione?.Trim() ?? "",
            Amount = csvDto.Importo,
            Type = type,
            Category = category,
            Merchant = merchant,
        };
    }
    
    public static ICollection<Transaction> ToTransactions(this ICollection<TransactionCsvModel> collection)
    {
        return collection.Select(x => x.ToTransaction()).ToList();
    }
}