using Microsoft.AspNetCore.Mvc;
using Risparmioforo.Domain.Account;
using Risparmioforo.Domain.Category;
using Risparmioforo.Services.AccountService;
using Risparmioforo.Services.CategoryService;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/account/");
        
        api.MapGet("/search", SearchCommand)
            .Produces<Result<Pagination<AccountDto>>>()
            .WithDescription("Search accounts")
            .WithOpenApi();
        
        api.MapPost("/create", CreateCommand)
            .Accepts<CreateAccountCommand>("application/json")
            .Produces<Result<AccountDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create an account")
            .WithOpenApi();
        
        api.MapPut("/update", UpdateCommand)
            .Accepts<UpdateAccountCommand>("application/json")
            .Produces<Result<AccountDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update an account")
            .WithOpenApi();
        
        api.MapDelete("/remove", RemoveCommand)
            .Accepts<RemoveAccountCommand>("application/json")
            .Produces<Result<bool>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Remove an account")
            .WithOpenApi();
    }

    private static async Task<Result<Pagination<AccountDto>>> SearchCommand(
        [FromServices] IAccountService accountService,
        [AsParameters] SearchCommand command, CancellationToken cancellationToken)
        => await accountService.Search(command, cancellationToken);

    private static async Task<IResult> CreateCommand(
        [FromServices] IAccountService accountService,
        [FromBody] CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await accountService.Create(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<AccountDto>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<AccountDto>.Failure(error)));
    }
    
    private static async Task<IResult> UpdateCommand(
        [FromServices] IAccountService accountService,
        [FromBody] UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await accountService.Update(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<AccountDto>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<AccountDto>.Failure(error)));
    }
    
    private static async Task<IResult> RemoveCommand(
        [FromServices] IAccountService accountService,
        [FromBody] RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await accountService.Remove(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<bool>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<bool>.Failure(error)));
    }
}