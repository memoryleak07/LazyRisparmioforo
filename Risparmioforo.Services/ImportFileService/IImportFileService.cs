using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.ImportFileService;

public interface IImportFileService
{
    Task<Result<bool>> ImportAsync(ImportFileCommand command, CancellationToken cancellationToken);
}