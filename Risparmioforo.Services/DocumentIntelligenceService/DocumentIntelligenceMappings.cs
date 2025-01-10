using Azure.AI.DocumentIntelligence;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public static class DocumentIntelligenceMappings
{
    public static ICollection<Transaction>? TryGetTransactions(this IReadOnlyList<AnalyzedDocument> analyzeResult)
    {
        var transactions = analyzeResult
            .Select(x => x.TryGetTransaction())
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();

        return transactions.Count > 0 ? transactions : null;
    }

    public static Transaction? TryGetTransaction(this AnalyzedDocument analyzedDocument)
    {
        DateOnly? registrationDate = analyzedDocument.TryGetTransactionDate();
        decimal? amount = analyzedDocument.TryGetTransactionAmount();
        
        // TODO: determine when to return null
        if (registrationDate == null || amount == null) 
            return null;

        TransactionType type = TransactionType.Expense;
        TransactionMethod method = analyzedDocument.TryGetTransactionMethod();
        TransactionMerchant? merchant = analyzedDocument.TryGetTransactionMerchant();
        ICollection<TransactionItem>? items = analyzedDocument.TryGetTransactionItems();
        
        return new Transaction
        {
            Description = "you wanna too much",
            ValueDate = DateOnly.FromDateTime(DateTime.Now),
            RegistrationDate = registrationDate.Value,
            Amount = amount.Value * -1,
            Type = type,
            Method = method,
            Merchant = merchant,
            Items = items,
        };
    }
    
    private static DateOnly? TryGetTransactionDate(this AnalyzedDocument analyzedDocument)
    {
        if (!analyzedDocument.Fields.TryGetValue("TransactionDate", out DocumentField transactionDate)
            || transactionDate.FieldType != DocumentFieldType.Date) 
            return null;
        
        if (!transactionDate.ValueDate.HasValue)
            return null;
        
        Console.WriteLine($"Transaction Date: '{transactionDate.ValueDate}', with confidence {transactionDate.Confidence}");
        
        return DateOnly.FromDateTime(transactionDate.ValueDate.Value.Date);
    }
    
    private static TransactionMerchant? TryGetTransactionMerchant(this AnalyzedDocument analyzedDocument)
    {
        TransactionMerchant? transactionMerchant = null;
        
        if (analyzedDocument.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField)
            && merchantNameField.FieldType == DocumentFieldType.String)
        {
            Console.WriteLine($"Merchant Name: '{merchantNameField.ValueString}', with confidence {merchantNameField.Confidence}");
            transactionMerchant = new TransactionMerchant { Name = merchantNameField.ValueString };
        }

        if (analyzedDocument.Fields.TryGetValue("MerchantAddress", out DocumentField merchantAddressField)
            && merchantAddressField.FieldType == DocumentFieldType.Address)
        {
            Console.WriteLine($"Merchant Address: '{merchantAddressField.Content}', with confidence {merchantAddressField.Confidence}");
            transactionMerchant ??= new TransactionMerchant(); 
            transactionMerchant.Location = merchantAddressField.Content; 
        }
        
        // TODO: determine operation type
        
        return transactionMerchant;
    }

    private static decimal? TryGetTransactionAmount(this AnalyzedDocument analyzedDocument)
    {
        if (!analyzedDocument.Fields.TryGetValue("Total", out DocumentField invoiceTotalField)
            || invoiceTotalField.FieldType != DocumentFieldType.Currency) 
            return null;
        
        Console.WriteLine($"Receipt Total: '{invoiceTotalField.ValueCurrency.CurrencySymbol}{invoiceTotalField.ValueCurrency.Amount}', with confidence {invoiceTotalField.Confidence}");
        return (decimal)invoiceTotalField.ValueCurrency.Amount;
    }
    
    private static TransactionMethod TryGetTransactionMethod(this AnalyzedDocument analyzedDocument)
    {
        if (!analyzedDocument.Fields.TryGetValue("Method", out DocumentField paymentMethodField)
            || paymentMethodField.FieldType != DocumentFieldType.String) 
            return TransactionMethod.Undefined;
        
        Console.WriteLine($"Transaction Method: '{paymentMethodField.ValueString}', with confidence {paymentMethodField.Confidence}");

        if (paymentMethodField.ValueString.Contains("cash", StringComparison.InvariantCultureIgnoreCase))
            return TransactionMethod.Cash;
        if (paymentMethodField.ValueString.Contains("card", StringComparison.InvariantCultureIgnoreCase))
            return TransactionMethod.Card;
        
        return TransactionMethod.Undefined;
    }
    
    private static ICollection<TransactionItem>? TryGetTransactionItems(this AnalyzedDocument analyzedDocument)
    {
        if (!analyzedDocument.Fields.TryGetValue("Items", out DocumentField itemsField)
            || itemsField.FieldType != DocumentFieldType.List
            || itemsField.ValueList.Count == 0) 
            return null;
        
        var transactionItems = new List<TransactionItem>();
        
        foreach (DocumentField itemField in itemsField.ValueList
                     .Where(x => x.FieldType == DocumentFieldType.Dictionary))
        {
            TransactionItem? item = null;
            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.ValueDictionary;
            
            if (itemFields.TryGetValue("Description", out DocumentField? itemDescriptionField)
                && itemDescriptionField.FieldType == DocumentFieldType.String)
            {
                Console.WriteLine($"  Description: '{itemDescriptionField.ValueString}', with confidence {itemDescriptionField.Confidence}");
                item = new TransactionItem { Item = itemDescriptionField.ValueString };
            }
            
            if (itemFields.TryGetValue("Quantity", out DocumentField? itemQuantityField)
                && itemQuantityField.FieldType == DocumentFieldType.Double
                && itemQuantityField.ValueDouble.HasValue)
            {
                Console.WriteLine($"  Quantity: '{itemQuantityField.ValueDouble}', with confidence {itemQuantityField.Confidence}");
                item ??= new TransactionItem(); 
                item.Quantity = (int)itemQuantityField.ValueDouble; 
            }
            
            if (itemFields.TryGetValue("Price", out DocumentField? itemPriceField)
                && itemPriceField.FieldType == DocumentFieldType.Currency)
            {
                Console.WriteLine($"  Price: '{itemPriceField.ValueCurrency.CurrencySymbol}{itemPriceField.ValueCurrency.Amount}', with confidence {itemPriceField.Confidence}");
                item ??= new TransactionItem(); 
                item.Price = (int)itemPriceField.ValueCurrency.Amount;
            }
            
            if (item is null) continue;
            transactionItems.Add(item);
        }

        return transactionItems.Count > 0 ? transactionItems : null;
    }
}