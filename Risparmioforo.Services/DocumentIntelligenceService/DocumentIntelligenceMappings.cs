using Azure.AI.DocumentIntelligence;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public static class DocumentIntelligenceMappings
{
    public static ICollection<Transaction>? TryGetTransactions(this IReadOnlyList<AnalyzedDocument> analyzedDocuments)
    {
        var transactions = analyzedDocuments
            .Select(document => document.TryGetTransaction())
            .Where(transaction => transaction is not null)
            .Select(transaction => transaction!)
            .ToList();

        return transactions.Count > 0 ? transactions : null;
    }

    public static Transaction? TryGetTransaction(this AnalyzedDocument analyzedDocument)
    {
        if (analyzedDocument.Confidence < 0.7) return null;
        
        DateOnly? registrationDate = analyzedDocument.TryGetTransactionDate();
        decimal? amount = analyzedDocument.TryGetTransactionAmount();
        
        // TODO: determine when to return null
        if (registrationDate is null || amount is null) 
            return null;
        
        return new Transaction
        {
            Description =  analyzedDocument.DocumentType,
            ValueDate = DateOnly.FromDateTime(DateTime.Now),
            RegistrationDate = registrationDate.Value,
            Amount = amount.Value * -1,
            Type = TransactionType.Expense,
            Method = TransactionMethod.Cash,
            Operation = TransactionOperation.Payment,
            Merchant = analyzedDocument.TryGetTransactionMerchant(),
            Items = analyzedDocument.TryGetTransactionItems(),
        };
    }
    
    private static decimal? TryGetTransactionAmount(this AnalyzedDocument analyzedDocument)
    {
        if (!analyzedDocument.Fields.TryGetValue("Total", out DocumentField invoiceTotalField)
            || invoiceTotalField.FieldType != DocumentFieldType.Currency) 
            return null;
        
        Console.WriteLine($"Receipt Total: '{invoiceTotalField.ValueCurrency.CurrencySymbol}{invoiceTotalField.ValueCurrency.Amount}', with confidence {invoiceTotalField.Confidence}");
        return (decimal)invoiceTotalField.ValueCurrency.Amount;
    }
    
    private static DateOnly? TryGetTransactionDate(this AnalyzedDocument analyzedDocument)
    {
        if (!analyzedDocument.Fields.TryGetValue("TransactionDate", out DocumentField transactionDate)
            || transactionDate.FieldType != DocumentFieldType.Date
            || !transactionDate.ValueDate.HasValue)
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
        
        return transactionMerchant;
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
            TransactionItem? transactionItem = null;
            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.ValueDictionary;
            
            if (itemFields.TryGetValue("Description", out DocumentField? itemDescriptionField)
                && itemDescriptionField.FieldType == DocumentFieldType.String)
            {
                Console.WriteLine($"  Description: '{itemDescriptionField.ValueString}', with confidence {itemDescriptionField.Confidence}");
                transactionItem = new TransactionItem { Item = itemDescriptionField.ValueString };
            }
            
            if (itemFields.TryGetValue("Quantity", out DocumentField? itemQuantityField)
                && itemQuantityField.FieldType == DocumentFieldType.Double
                && itemQuantityField.ValueDouble.HasValue)
            {
                Console.WriteLine($"  Quantity: '{itemQuantityField.ValueDouble}', with confidence {itemQuantityField.Confidence}");
                transactionItem ??= new TransactionItem(); 
                transactionItem.Quantity = (int)itemQuantityField.ValueDouble; 
            }
            
            if (itemFields.TryGetValue("Price", out DocumentField? itemPriceField)
                && itemPriceField.FieldType == DocumentFieldType.Currency)
            {
                Console.WriteLine($"  Price: '{itemPriceField.ValueCurrency.CurrencySymbol}{itemPriceField.ValueCurrency.Amount}', with confidence {itemPriceField.Confidence}");
                transactionItem ??= new TransactionItem(); 
                transactionItem.Price = (int)itemPriceField.ValueCurrency.Amount;
            }
            
            if (transactionItem is null) continue;
            transactionItems.Add(transactionItem);
        }

        return transactionItems.Count > 0 ? transactionItems : null;
    }
}