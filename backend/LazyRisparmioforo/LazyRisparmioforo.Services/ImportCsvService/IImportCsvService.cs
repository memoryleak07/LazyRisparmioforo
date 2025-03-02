using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Shared.Shared;

namespace ImportCsvService;

public interface IImportCsvService
{
    Task<Result> ImportCsvAsync(UploadFileCommand command, CancellationToken cancellationToken);
}