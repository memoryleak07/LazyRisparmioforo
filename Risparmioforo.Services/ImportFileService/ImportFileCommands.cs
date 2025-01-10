namespace Risparmioforo.Services.ImportFileService;

public class ImportFileCommand
{
    public string ContentType { get; set; }
    public byte[] FileBytes { get; set; }
    public string FileName { get; set; } 
    public long FileLength { get; set; } 
}
