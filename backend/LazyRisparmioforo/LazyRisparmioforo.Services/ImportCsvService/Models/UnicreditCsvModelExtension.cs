using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;

namespace ImportCsvService.Models;

public static class UnicreditCsvModelExtension
{
    public static Transaction ToTransaction(this UnicreditCsvModel model)
    {
        return new Transaction
        {
            RegistrationDate = model.DataRegistraz,
            ValueDate = model.DataValuta,
            Description = model.Descrizione?.Trim() ?? "",
            Amount = model.Importo,
            Flow = model.Importo switch
            {
                < 0 => Flow.Expense,
                > 0 => Flow.Income,
                _ => Flow.Undefined
            }
        };
    }

    public static ICollection<Transaction> ToTransactions(this ICollection<UnicreditCsvModel> collection)
        => collection.Select(x => x.ToTransaction()).ToList();
}