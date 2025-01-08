using Microsoft.AspNetCore.Mvc;
using Risparmioforo.Services.ImportFileService;
using Risparmioforo.Shared.Base;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;

namespace Risparmioforo.Api.Endpoints;

public static class ImportFileEndpoints
{
    public static void MapImportFileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/import");
        
        api.MapPut("/", ImportCommand)
            .Accepts<UploadFileViewModel>("multipart/form-data")
            .Produces<Result<bool>>(StatusCodes.Status200OK)
            .Produces<Result<bool>>(StatusCodes.Status400BadRequest)
            .WithDescription("Import bank transactions")
            .WithOpenApi()
            .DisableAntiforgery();
    }

    private class UploadFileViewModel
    {
        public required IFormFile FormFile { get; set; }
    }
    private static async Task<IResult> ImportCommand(
        [FromServices] IImportFileService importFileService,
        [FromForm] UploadFileViewModel request,
        CancellationToken cancellationToken)
    {
        var command =  new ImportFileCommand
        {
            FileStream = new StreamReader(request.FormFile.OpenReadStream()),
            ContentType = request.FormFile.ContentType,
            FileName = request.FormFile.FileName,
            FileLength = request.FormFile.Length
        };
        
        var result = await importFileService.ImportAsync(command, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<bool>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<bool>.Failure(error)));
    }
}