using Microsoft.AspNetCore.Http;

namespace LazyRisparmioforo.Domain.Commands;

public record UploadFileViewModel(IFormFile FormFile);

public class UploadFileCommand
{
    public string ContentType { get; set; }
    public Stream FileStream { get; set; }
    public string FileName { get; set; } 
    public long FileLength { get; set; } 
}

public static class UploadFileCommandExtension
{
    public static UploadFileCommand ToImportFileCommand(this UploadFileViewModel viewModel)
    {
        return new UploadFileCommand
        {
            FileStream = viewModel.FormFile.OpenReadStream(),
            ContentType = viewModel.FormFile.ContentType,
            FileName = viewModel.FormFile.FileName,
            FileLength = viewModel.FormFile.Length
        };
    }
}
