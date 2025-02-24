using CsvHelper.Configuration;

namespace ImportCsvService.Models;

// Data Registrazione;Data valuta;Descrizione;Importo (EUR);
public class UnicreditCsvModel
{
    public DateOnly DataRegistraz {get; set;}
    public DateOnly DataValuta {get; set;}
    public string Descrizione { get; set; } = string.Empty;
    public decimal Importo {get; set;}
}

public sealed class UnicreditCsvModelMap : ClassMap<UnicreditCsvModel>
{
    public UnicreditCsvModelMap()
    {
        Map(m => m.DataRegistraz)
            .Name("Data Registrazione").TypeConverterOption.Format("dd.MM.yyyy");
        
        Map(m => m.DataValuta)
            .Name("Data valuta").TypeConverterOption.Format("dd.MM.yyyy");
        
        Map(m => m.Descrizione)
            .Name("Descrizione");
        
        Map(m => m.Importo)
            .Name("Importo (EUR)");
    }
}