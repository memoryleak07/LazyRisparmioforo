using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public interface IDocumentIntelligenceService
{
    Task<Result<bool>> UploadDocumentAsync(StreamReader streamReader, CancellationToken cancellationToken);
}