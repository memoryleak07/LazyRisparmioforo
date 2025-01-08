using CsvHelper.Configuration;

namespace Risparmioforo.Services.UnicreditCsvService;

public sealed class UnicreditCsvModelMap : ClassMap<UnicreditCsvModel>
{
    public UnicreditCsvModelMap()
    {
        // AutoMap(CultureInfo.InvariantCulture);
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