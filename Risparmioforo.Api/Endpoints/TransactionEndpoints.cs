using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Risparmioforo.Api.Services.TransactionService;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/transactions");
        
        api.MapGet("/search", SearchCommand)
            .Produces<Result<Pagination<Transaction>>>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Search transactions")
            .WithOpenApi();
        
        api.MapPost("/create", CreateCommand)
            .Produces<Result<Transaction>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a transaction")
            .WithOpenApi();
        
        api.MapPut("/update", UpdateCommand)
            .Produces<Result<Transaction>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update a transaction")
            .WithOpenApi();
        
        api.MapDelete("/remove", RemoveCommand)
            .Produces<Ok>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Remove a transaction")
            .WithOpenApi();
    }

    private static async Task<Result<Pagination<Transaction>>> SearchCommand(
        [FromServices] ITransactionService transactionService,
        [FromQuery] string? query, int pageIndex = 0, int pageSize = 10) 
        => await transactionService.Search(query, pageIndex, pageSize);

    private static async Task<IResult> CreateCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await transactionService.Create(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<Transaction>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<Transaction>.Failure(error)));
    }

    private static async Task<IResult> UpdateCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await transactionService.Update(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<Transaction>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<Transaction>.Failure(error)));
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