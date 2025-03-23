using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Domain.Queries;
using LazyRisparmioforo.Shared.Shared;
using Microsoft.AspNetCore.Mvc;
using MinimalHelpers.FluentValidation;
using TransactionService;

namespace LazyRisparmioforo.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/transactions/");

        api.MapGet("/{id}", async (
                [FromServices] ITransactionService transactionService,
                [FromRoute] int id,
                CancellationToken cancellationToken) =>
            {
                var result = await transactionService.GetAsync(new TransactionGetCommand(id), cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .Produces<Transaction>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Get transaction by id")
            .WithOpenApi();

        api.MapGet("/search", async (
                [FromServices] ITransactionService transactionService,
                [AsParameters] TransactionSearchQuery query,
                CancellationToken cancellationToken) => await transactionService.SearchAsync(query, cancellationToken))
            .Produces<Pagination<Transaction>>()
            .Produces(StatusCodes.Status200OK)
            .WithValidation<TransactionSearchQuery>()
            .WithDescription("Search transactions")
            .WithOpenApi();

        api.MapPost("/create", async (
                [FromServices] ITransactionService transactionService,
                [FromBody] TransactionCreateCommand command,
                CancellationToken cancellationToken) =>
            {
                var result = await transactionService.CreateAsync(command, cancellationToken);
                return result.IsSuccess ? Results.Created() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<TransactionCreateCommand>()
            .WithDescription("Create a transaction")
            .WithOpenApi();

        api.MapPut("/update", async (
                [FromServices] ITransactionService transactionService,
                [FromBody] TransactionUpdateCommand command,
                CancellationToken cancellationToken) =>
            {
                var result = await transactionService.UpdateAsync(command, cancellationToken);
                return result.IsSuccess ? Results.Created() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<TransactionUpdateCommand>()
            .WithDescription("Update a transaction")
            .WithOpenApi();

        api.MapDelete("/remove", async (
                [FromServices] ITransactionService transactionService,
                [AsParameters] TransactionRemoveCommand request,
                CancellationToken cancellationToken) =>
            {
                var result = await transactionService.RemoveAsync(request, cancellationToken);
                return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<TransactionRemoveCommand>()
            .WithDescription("Remove a transaction")
            .WithOpenApi();
    }
}