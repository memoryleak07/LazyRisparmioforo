using System.Globalization;
using CsvHelper.Configuration;

namespace Risparmioforo.Services.TransactionService;

public class TransactionCsvModel
{
    public DateOnly DataRegistrazione {get; set;}
    public DateOnly DataValuta {get; set;}
    public string? Descrizione {get; set;}
    public decimal Importo {get; set;}
}

public sealed class TransactionCsvModelMap : ClassMap<TransactionCsvModel>
{
    public TransactionCsvModelMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.DataRegistrazione).Name("Data Registrazione");
        Map(m => m.DataValuta).Name("Data valuta");
        Map(m => m.Descrizione).Name("Descrizione");
        Map(m => m.Importo).Name("Importo (EUR)");
    }
}