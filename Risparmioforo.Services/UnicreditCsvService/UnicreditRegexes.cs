using System.Text.RegularExpressions;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.UnicreditCsvService;

public static class UnicreditRegexes
{
    public static readonly Dictionary<TransactionOperation, Regex> CategoryPatterns = new()
    {
        [TransactionOperation.Salary] = new(@"VOSTRI\s+EMOLUMENTI", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Payment] = new(@"\bPAGAMENTO(?!\s+RATA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), // ?!\s+RATA avoids PAGAMENTO+RATA to be handled as Payment
        [TransactionOperation.Credit] = new(@"ACCREDITI\s+VARI", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Transfer] = new(@"BONIFICO", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Debit] = new(@"\b(ADDEBITO\s+SEPA\s+DD|RIMBORSO\s+PRESTITO|PAGAMENTO\s+RATA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Fee] = new(@"\b(MY\s+GENIUS|COMMISSIONI|IMPOSTA\s+BOLLO\s+CONTO\s+CORRENTE|COMMISSIONI\s+-\s+PROVVIGIONI\s+-\s+SPESE)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Withdraw] = new(@"PRELIEVO\s+(?:BANCOMAT|MASTERCARD|VISA|SMART)(?!.*COMMISSIONI)", RegexOptions.Compiled | RegexOptions.IgnoreCase),
    };
    public static readonly Regex MerchantEcommercePattern = new(@"\b(E-Commerce)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex MerchantContactlessPattern = new(@"\b(Contactless)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex CardNumberPattern = new(@"\bCARTA \*(\d+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public static readonly TransactionOperation[] IncomeCategories =
    [
        TransactionOperation.Salary,
        TransactionOperation.Credit,
        TransactionOperation.Transfer
    ];

    public static readonly TransactionOperation[] ExpenseCategories =
    [
        TransactionOperation.Debit,
        TransactionOperation.Fee,
        TransactionOperation.Payment,
        TransactionOperation.Withdraw,
        TransactionOperation.Transfer
    ];
}