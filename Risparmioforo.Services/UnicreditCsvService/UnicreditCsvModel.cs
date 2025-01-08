namespace Risparmioforo.Services.UnicreditCsvService;

public class UnicreditCsvModel
{
    public DateOnly DataRegistraz {get; set;}
    public DateOnly DataValuta {get; set;}
    public string? Descrizione {get; set;}
    public decimal Importo {get; set;}
}