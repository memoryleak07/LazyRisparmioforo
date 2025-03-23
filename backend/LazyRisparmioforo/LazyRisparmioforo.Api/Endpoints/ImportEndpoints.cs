using ImportCsvService;
using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using MinimalHelpers.FluentValidation;

namespace LazyRisparmioforo.Api.Endpoints;

public static class ImportEndpoints
{
    public static void MapImportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/import");

        api.MapPost("/csv", async (
                [FromServices] IImportCsvService importCsvService,
                [FromForm] UploadFileViewModel request,
                CancellationToken cancellationToken) =>
            {
                var result = await importCsvService.ImportCsvAsync(request.ToImportFileCommand(), cancellationToken);
                return result.IsSuccess ? Results.Created() : result.ToProblemDetails();
            })
            .Accepts<UploadFileViewModel>("multipart/form-data")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<UploadFileViewModel>()
            .WithDescription("Import csv bank transactions")
            .WithOpenApi()
            .DisableAntiforgery();
    }
}