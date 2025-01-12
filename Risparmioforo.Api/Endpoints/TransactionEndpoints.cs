using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Services.TransactionService;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/transactions/");
        
        api.MapGet("/search", SearchCommand)
            .Produces<Result<Pagination<TransactionDto>>>()
            .WithDescription("Search transactions")
            .WithOpenApi();
        
        api.MapPost("/create", CreateCommand)
            .Produces<Result<TransactionDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a transaction")
            .WithOpenApi();
        
        api.MapPut("/update", UpdateCommand)
            .Produces<Result<TransactionDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update a transaction")
            .WithOpenApi();
        
        api.MapDelete("/remove", RemoveCommand)
            .Produces<Ok>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Remove a transaction")
            .WithOpenApi();
    }

    private static async Task<Result<Pagination<TransactionDto>>> SearchCommand(
        [FromServices] ITransactionService transactionService,
        [AsParameters] SearchCommand command, CancellationToken cancellationToken)
        => await transactionService.Search(command, cancellationToken);

    private static async Task<IResult> CreateCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await transactionService.Create(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<TransactionDto>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<TransactionDto>.Failure(error)));
    }

    private static async Task<IResult> UpdateCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await transactionService.Update(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<TransactionDto>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<TransactionDto>.Failure(error)));
    }
    
    private static async Task<IResult> RemoveCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] RemoveTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await transactionService.Remove(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<bool>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<bool>.Failure(error)));
    }
}