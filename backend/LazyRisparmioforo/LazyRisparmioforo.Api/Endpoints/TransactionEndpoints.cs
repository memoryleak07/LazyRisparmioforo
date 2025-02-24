using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using MinimalHelpers.FluentValidation;
using TransactionService;

namespace LazyRisparmioforo.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/transactions/");
        
        api.MapGet("/{id}", GetCommand)
            .Produces<Transaction>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Get transaction by id")
            .WithOpenApi();
        
        api.MapGet("/search", SearchCommand)
            .Produces<Pagination<Transaction>>()
            .Produces(StatusCodes.Status200OK)
            .WithValidation<TransactionSearchCommand>()
            .WithDescription("Search transactions")
            .WithOpenApi();

        api.MapPost("/create", CreateCommand)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<TransactionCreateCommand>()
            .WithDescription("Create a transaction")
            .WithOpenApi();
        
        api.MapPut("/update", UpdateCommand)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<TransactionUpdateCommand>()
            .WithDescription("Update a transaction")
            .WithOpenApi();
        
        api.MapDelete("/remove", RemoveCommand)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<TransactionRemoveCommand>()
            .WithDescription("Remove a transaction")
            .WithOpenApi();
    }

    private static async Task<Pagination<Transaction>> SearchCommand(
        [FromServices] ITransactionService transactionService,
        [AsParameters] TransactionSearchCommand command, 
        CancellationToken cancellationToken)
        => await transactionService.SearchAsync(command, cancellationToken);

    private static async Task<IResult> GetCommand(
        [FromServices] ITransactionService transactionService,
        [FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var result = await transactionService.GetAsync(new TransactionGetCommand(id), cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
    
    private static async Task<IResult> CreateCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] TransactionCreateCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await transactionService.CreateAsync(command, cancellationToken);
        return result.IsSuccess ? Results.Created() : result.ToProblemDetails();
    }
    
    private static async Task<IResult> UpdateCommand(
        [FromServices] ITransactionService transactionService,
        [FromBody] TransactionUpdateCommand command, CancellationToken cancellationToken)
    {
        var result = await transactionService.UpdateAsync(command, cancellationToken);
        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    private static async Task<IResult> RemoveCommand(
        [FromServices] ITransactionService transactionService,
        [AsParameters] TransactionRemoveCommand request, 
        CancellationToken cancellationToken)
    {
        var result = await transactionService.RemoveAsync(request, cancellationToken);
        return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
    }
}