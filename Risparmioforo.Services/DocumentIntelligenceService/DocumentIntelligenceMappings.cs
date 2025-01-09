using Azure.AI.DocumentIntelligence;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public static class DocumentIntelligenceMappings
{
    public static ICollection<Transaction> TryGetTransactions(this IReadOnlyList<AnalyzedDocument> analyzeResult)
    {
        return analyzeResult
            .Select(x => x.TryGetTransaction())
            .ToList();
    }

    public static Transaction TryGetTransaction(this AnalyzedDocument analyzedDocument)
    {
        return new Transaction
        {
            Description = "you wanna too much",
            Merchant = TryGetTransactionMerchant(analyzedDocument),
            Amount = TryGetTransactionAmount(analyzedDocument),
            Items = TryGetTransactionItems(analyzedDocument),
            RegistrationDate = TryGetTransactionDate(analyzedDocument),
        };
    }
    
    private static DateOnly TryGetTransactionDate(this AnalyzedDocument analyzedDocument)
    {
        return DateOnly.MinValue;
    }
    
    private static TransactionMerchant TryGetTransactionMerchant(this AnalyzedDocument analyzedDocument)
    {
        var transactionMerchant = new TransactionMerchant();
        
        if (analyzedDocument.Fields.TryGetValue("VendorName", out DocumentField vendorNameField)
            && vendorNameField.FieldType == DocumentFieldType.String)
        {
            string vendorName = vendorNameField.ValueString;
            transactionMerchant.Name = vendorName;
            Console.WriteLine($"Vendor Name: '{vendorName}', " +
                              $"with confidence {vendorNameField.Confidence}");
        }

        if (analyzedDocument.Fields.TryGetValue("CustomerName", out DocumentField customerNameField)
            && customerNameField.FieldType == DocumentFieldType.String)
        {
            string customerName = customerNameField.ValueString;
            Console.WriteLine($"Customer Name: '{customerName}', " +
                              $"with confidence {customerNameField.Confidence}");
        }
            
        // if (analyzedDocument.Fields.TryGetValue("CustomerName", out DocumentField customerLocationField)
        //     && customerLocationField.FieldType == DocumentFieldType.String)
        // {
        //     string customerName = customerLocationField.ValueString;
        //     Console.WriteLine($"Customer Location: '{customerName}', with confidence {customerNameField.Confidence}");
        // }

        return transactionMerchant;
    }

    private static decimal TryGetTransactionAmount(this AnalyzedDocument analyzedDocument)
    {
        if (analyzedDocument.Fields.TryGetValue("Total", out DocumentField invoiceTotalField)
            && invoiceTotalField.FieldType == DocumentFieldType.Currency)
        {
            CurrencyValue receiptTotal = invoiceTotalField.ValueCurrency;
            Console.WriteLine($"Receipt Total: '{receiptTotal.CurrencySymbol}{receiptTotal.Amount}', " +
                              $"with confidence {invoiceTotalField.Confidence}");

            return (decimal)receiptTotal.Amount;
        }
        return 0;
    }
    
    private static ICollection<TransactionItem>? TryGetTransactionItems(this AnalyzedDocument analyzedDocument)
    {

        if (!analyzedDocument.Fields.TryGetValue("Items", out DocumentField itemsField)
            || itemsField.FieldType != DocumentFieldType.List) 
            return null!;
        
        var transactionItems = new List<TransactionItem>();
        
        foreach (DocumentField itemField in itemsField.ValueList)
        {
            if (itemField.FieldType != DocumentFieldType.Dictionary) continue;
                
            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.ValueDictionary;
            
            if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField)
                && itemDescriptionField.FieldType == DocumentFieldType.String)
            {
                string itemDescription = itemDescriptionField.ValueString;
                Console.WriteLine($"  Description: '{itemDescription}', " +
                                  $"with confidence {itemDescriptionField.Confidence}");
            }
            
            if (itemFields.TryGetValue("TotalPrice", out DocumentField itemAmountField)
                && itemAmountField.FieldType == DocumentFieldType.Currency)
            {
                CurrencyValue itemAmount = itemAmountField.ValueCurrency;
                Console.WriteLine($"  Price: '{itemAmount.CurrencySymbol}{itemAmount.Amount}', " +
                                  $"with confidence {itemAmountField.Confidence}");
            }
        }

        return transactionItems;
    }
}