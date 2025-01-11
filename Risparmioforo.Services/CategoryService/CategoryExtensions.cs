using System.Text.RegularExpressions;
using Risparmioforo.Domain.Category;
using Risparmioforo.Domain.Common;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.CategoryService;

public static class CategoryExtensions
{
    public static void SetCategories(this ICollection<Transaction>? transactions, Category[] categories)
    {
        if (transactions is null || categories.Length == 0)
            return;
        
        foreach (var transaction in transactions)
        {
            // We take the input string from different sources:
            //  - When is Expense we search for a match in the transaction.Merchant.Name (eg Amazon)
            //  - When is Income we search directly in the transaction.Description
            string inputString = transaction.Flow switch
            {
                Flow.Expense => transaction.Merchant?.Name ?? transaction.Description,
                Flow.Income => transaction.Description,
                _ => ""
            };

            if (string.IsNullOrEmpty(inputString))
                return;
            
            // Then we try if any of the user category's keywords match the input string
            Category? matchCategory = categories
                .Where(category => category.Flow == transaction.Flow)
                .FirstOrDefault(category => category.Keywords
                    .Any(keyword => Regex.IsMatch(inputString, keyword, RegexOptions.IgnoreCase)));

            if (matchCategory != null)
            {
                transaction.Category = matchCategory.Name;
            }
        }
    }
}