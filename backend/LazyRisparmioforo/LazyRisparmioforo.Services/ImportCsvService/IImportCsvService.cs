using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Shared;

namespace ImportCsvService;

public interface IImportCsvService
{
    Task<Result> ImportCsvAsync(UploadFileCommand command, CancellationToken cancellationToken);
}