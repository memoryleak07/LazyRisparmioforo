namespace Risparmioforo.Services.ImportFileService;

public class ImportFileCommand
{
    public StreamReader FileStream { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; } 
    public long FileLength { get; set; } 
}
