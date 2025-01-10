using System.Text.RegularExpressions;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.UnicreditCsvService;

public static class UnicreditRegexes
{
    public static readonly Dictionary<TransactionOperation, Regex> TransactionOperationPatterns = new()
    {
        [TransactionOperation.Payment] = new(@"\bPAGAMENTO(?!\s+RATA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), // ?!\s+RATA avoids PAGAMENTO+RATA to be handled as Payment
        [TransactionOperation.Credit] = new(@"ACCREDITI\s+VARI", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Transfer] = new(@"BONIFICO|VOSTRI\s+EMOLUMENTI", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Debit] = new(@"\b(ADDEBITO\s+SEPA\s+DD|RIMBORSO\s+PRESTITO|PAGAMENTO\s+RATA)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Fee] = new(@"\b(MY\s+GENIUS|COMMISSIONI|IMPOSTA\s+BOLLO\s+CONTO\s+CORRENTE|COMMISSIONI\s+-\s+PROVVIGIONI\s+-\s+SPESE)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        [TransactionOperation.Withdraw] = new(@"PRELIEVO\s+(?:BANCOMAT|MASTERCARD|VISA|SMART)(?!.*COMMISSIONI)", RegexOptions.Compiled | RegexOptions.IgnoreCase),
    };
    public static readonly Regex EcommercePattern = new(@"\b(E-Commerce)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex ContactlessPattern = new(@"\b(Contactless)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex CardNumberPattern = new(@"\bCARTA \*(\d+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public static readonly TransactionOperation[] IncomeOperationPatterns =
    [
        TransactionOperation.Credit,
        TransactionOperation.Transfer
    ];

    public static readonly TransactionOperation[] ExpenseOperationPatterns =
    [
        TransactionOperation.Debit,
        TransactionOperation.Fee,
        TransactionOperation.Payment,
        TransactionOperation.Withdraw,
        TransactionOperation.Transfer
    ];
}