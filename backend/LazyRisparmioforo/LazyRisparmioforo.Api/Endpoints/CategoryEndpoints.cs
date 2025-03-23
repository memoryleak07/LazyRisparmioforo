using CategoryService;
using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LazyRisparmioforo.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/categories/");
        
        api.MapGet("/all", async (
                    [FromServices] ICategoryService categoryService,
                    CancellationToken cancellationToken) => await categoryService.AllAsync(cancellationToken))
            .Produces<ICollection<Category>>()
            .Produces(StatusCodes.Status200OK)
            .WithDescription("Get all categories")
            .WithOpenApi();

        api.MapPost("/predict", async (
                [FromServices] ICategoryService categoryService,
                [AsParameters] CategoryPredictCommand command,
                CancellationToken cancellationToken) => 
            {
                var result = await categoryService.PredictAsync(command, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .Produces<CategoryPredictResult>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Predict a category from a text input")
            .WithOpenApi();
    }
}