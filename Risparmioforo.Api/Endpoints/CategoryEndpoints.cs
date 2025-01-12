using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Risparmioforo.Domain.Category;
using Risparmioforo.Services.CategoryService;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/category/");
        
        api.MapGet("/search", SearchCommand)
            .Produces<Result<Pagination<Category>>>()
            .WithDescription("Search categories")
            .WithOpenApi();
        
        api.MapPost("/create", CreateCommand)
            .Accepts<CreateCategoryCommand>("application/json")
            .Produces<Result<Category>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a category")
            .WithOpenApi();
        
        api.MapPut("/update", UpdateCommand)
            .Accepts<UpdateCategoryCommand>("application/json")
            .Produces<Result<Category>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update a category")
            .WithOpenApi();
        
        api.MapDelete("/remove", RemoveCommand)
            .Accepts<RemoveCategoryCommand>("application/json")
            .Produces<Ok>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Remove a category")
            .WithOpenApi();
    }

    private static async Task<Result<Pagination<Category>>> SearchCommand(
        [FromServices] ICategoryService categoryService,
        [AsParameters] SearchCommand command, CancellationToken cancellationToken)
        => await categoryService.Search(command, cancellationToken);

    private static async Task<IResult> CreateCommand(
        [FromServices] ICategoryService categoryService,
        [FromBody] CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryService.Create(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<Category>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<Category>.Failure(error)));
    }
    
    private static async Task<IResult> UpdateCommand(
        [FromServices] ICategoryService categoryService,
        [FromBody] UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryService.Update(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<Category>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<Category>.Failure(error)));
    }
    
    private static async Task<IResult> RemoveCommand(
        [FromServices] ICategoryService categoryService,
        [FromBody] RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryService.Remove(request, cancellationToken);
        return result.Map<IResult>(
            onSuccess: value => TypedResults.Ok(Result<bool>.Success(value)),
            onFailure: error => TypedResults.BadRequest(Result<bool>.Failure(error)));
    }
}