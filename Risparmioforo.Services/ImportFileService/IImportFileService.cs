using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.ImportFileService;

public interface IImportFileService
{
    Task<Result<bool>> ImportCsvAsync(ImportFileCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> ImportPhotoAsync(ImportFileCommand request, CancellationToken cancellationToken);
}