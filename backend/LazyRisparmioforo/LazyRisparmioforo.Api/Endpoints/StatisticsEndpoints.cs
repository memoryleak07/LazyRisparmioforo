using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using MinimalHelpers.FluentValidation;
using StatisticsService;

namespace LazyRisparmioforo.Api.Endpoints;

public static class StatisticsEndpoints
{
    public static void MapStatisticEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api/statistics/");
        
        api.MapGet("/summary", SummaryCommand)
            .Produces<SummaryDto>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<StatRequestCommand>()
            .WithDescription("Get total amount income and expense within a date range.")
            .WithOpenApi();
        
        api.MapGet("/summary-monthly", SummaryMonthlyCommand)
            .Produces<ICollection<SummaryMonthlyDto>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<StatRequestCommand>()
            .WithDescription("Get total amount income and expense grouped by month within a date range.")
            .WithOpenApi();
        
        api.MapGet("/spent-per-category", SpentPerCategoryCommand)
            .Produces<ICollection<CategoryAmountDto>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<StatRequestCommand>()
            .WithDescription("Get the total amount spent per category within a date range.")
            .WithOpenApi();
    }

    private static async Task<IResult> SummaryCommand(
        [FromServices] IStatisticService statisticService,
        [AsParameters] StatRequestCommand requestCommand,
        CancellationToken cancellationToken)
    {
        var result = await statisticService.SummaryAsync(requestCommand, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
    
    private static async Task<IResult> SummaryMonthlyCommand(
        [FromServices] IStatisticService statisticService,
        [AsParameters] StatRequestCommand requestCommand,
        CancellationToken cancellationToken)
    {
        var result = await statisticService.SummaryMonthlyAsync(requestCommand, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
    
    private static async Task<IResult> SpentPerCategoryCommand(
        [FromServices] IStatisticService statisticService,
        [AsParameters] StatRequestCommand requestCommand,
        CancellationToken cancellationToken)
    {
        var result = await statisticService.SpentPerCategoryAsync(requestCommand, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}