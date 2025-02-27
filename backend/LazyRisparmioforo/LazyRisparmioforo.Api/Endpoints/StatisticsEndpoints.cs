using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using MinimalHelpers.FluentValidation;
using StatisticsService;

namespace LazyRisparmioforo.Api.Endpoints;

public static class StatisticsEndpoints
{
    public static void MapStatisticEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/statistics/");
        
        api.MapGet("/total-amount", TotalAmountCommand)
            .Produces<decimal>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<StatCommand>()
            .WithDescription("Get total amount within a date range")
            .WithOpenApi();
        
        api.MapGet("/spent-per-category", SpentPerCategoryCommand)
            .Produces<ICollection<StatResult>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<StatCommand>()
            .WithDescription("Get the total amount spent per category within a date range.")
            .WithOpenApi();
    }

    private static async Task<IResult> TotalAmountCommand(
        [FromServices] IStatisticService statisticService,
        [AsParameters] StatCommand command,
        CancellationToken cancellationToken)
    {
        var result = await statisticService.TotalAmountAsync(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
    
    private static async Task<IResult> SpentPerCategoryCommand(
        [FromServices] IStatisticService statisticService,
        [AsParameters] StatCommand command,
        CancellationToken cancellationToken)
    {
        var result = await statisticService.SpentPerCategoryAsync(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}