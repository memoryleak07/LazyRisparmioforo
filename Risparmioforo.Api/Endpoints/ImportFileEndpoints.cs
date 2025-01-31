﻿using Microsoft.AspNetCore.Mvc;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Services.ImportFileService;
using Risparmioforo.Shared.Base;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;

namespace Risparmioforo.Api.Endpoints;

public static class ImportFileEndpoints
{
    public static void MapImportFileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/import");
        
        api.MapPut("/csv", ImportCsvCommand)
            .Accepts<UploadFileViewModel>("multipart/form-data")
            .Produces<Result<Pagination<TransactionDto>>>(StatusCodes.Status200OK)
            .Produces<Result<Pagination<TransactionDto>>>(StatusCodes.Status400BadRequest)
            .WithDescription("Import csv bank transactions")
            .WithOpenApi()
            .DisableAntiforgery();
        
        api.MapPut("/photo", ImportReceiptCommand)
            .Accepts<UploadFileViewModel>("multipart/form-data")
            .Produces<Result<Pagination<TransactionDto>>>(StatusCodes.Status200OK)
            .Produces<Result<Pagination<TransactionDto>>>(StatusCodes.Status400BadRequest)
            .WithDescription("Upload photo of receipts documents")
            .WithOpenApi()
            .DisableAntiforgery();
    }

    private class UploadFileViewModel
    {
        public required IFormFile FormFile { get; set; }
    }

    private static ImportFileCommand ToImportFileCommand(this UploadFileViewModel model)
    {
        using var memoryStream = new MemoryStream();
        model.FormFile.CopyTo(memoryStream);
        return new ImportFileCommand
        {
            FileBytes = memoryStream.ToArray(),
            ContentType = model.FormFile.ContentType,
            FileName = model.FormFile.FileName,
            FileLength = model.FormFile.Length
        };
    }
    
    private static async Task<IResult> ImportCsvCommand(
        [FromServices] IImportFileService importFileService,
        [FromForm] UploadFileViewModel request,
        CancellationToken cancellationToken)
    {
        var result = await importFileService.ImportCsvAsync(request.ToImportFileCommand(), cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<Pagination<TransactionDto>>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<Pagination<TransactionDto>>.Failure(error)));
    }
    
    private static async Task<IResult> ImportReceiptCommand(
        [FromServices] IImportFileService importFileService,
        [FromForm] UploadFileViewModel request,
        CancellationToken cancellationToken)
    {
        var result = await importFileService.ImportReceiptDocumentsAsync(request.ToImportFileCommand(), cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<Pagination<TransactionDto>>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<Pagination<TransactionDto>>.Failure(error)));
    }
}