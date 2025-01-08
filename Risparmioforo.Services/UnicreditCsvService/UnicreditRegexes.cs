using System.Text.RegularExpressions;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.UnicreditCsvService;

public static class UnicreditRegexes
{
    public static readonly Dictionary<TransactionCategory, Regex> CategoryPatterns = new()
    {
        [TransactionCategory.Salary] = new(@"VOSTRI\s+EMOLUMENTI", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionCategory.Payment] = new(@"\bPAGAMENTO(?!\s+RATA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), // ?!\s+RATA avoids PAGAMENTO+RATA to be handled as Payment
        [TransactionCategory.Credit] = new(@"ACCREDITI\s+VARI", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionCategory.Transfer] = new(@"BONIFICO", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionCategory.Debit] = new(@"\b(ADDEBITO\s+SEPA\s+DD|RIMBORSO\s+PRESTITO|PAGAMENTO\s+RATA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionCategory.Fees] = new(@"\b(MY\s+GENIUS|COMMISSIONI|IMPOSTA\s+BOLLO\s+CONTO\s+CORRENTE|COMMISSIONI\s+-\s+PROVVIGIONI\s+-\s+SPESE)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionCategory.Withdrawal] = new(@"PRELIEVO\s+(?:BANCOMAT|MASTERCARD|VISA|SMART)(?!.*COMMISSIONI)", RegexOptions.Compiled | RegexOptions.IgnoreCase),
    };
    public static readonly Regex MerchantEcommercePattern = new(@"\b(E-Commerce)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex MerchantContactlessPattern = new(@"\b(Contactless)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex CardNumberPattern = new(@"\bCARTA \*(\d+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public static readonly TransactionCategory[] IncomeCategories =
    [
        TransactionCategory.Salary,
        TransactionCategory.Credit,
        TransactionCategory.Transfer
    ];

    public static readonly TransactionCategory[] ExpenseCategories =
    [
        TransactionCategory.Debit,
        TransactionCategory.Fees,
        TransactionCategory.Payment,
        TransactionCategory.Withdrawal,
        TransactionCategory.Transfer
    ];
}