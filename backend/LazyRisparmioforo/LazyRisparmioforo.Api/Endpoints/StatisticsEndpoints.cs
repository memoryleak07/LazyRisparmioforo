using LazyRisparmioforo.Api.Extensions;
using LazyRisparmioforo.Domain.Queries;
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

        api.MapGet("/summary", async (
                [FromServices] IStatisticService statisticService,
                [AsParameters] DateRangeQuery requestCommand,
                CancellationToken cancellationToken) =>
            {
                var result = await statisticService.SummaryAsync(requestCommand, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .Produces<SummaryDto>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<DateRangeQuery>()
            .WithDescription("Get total amount income and expense within a date range.")
            .WithOpenApi();

        api.MapGet("/summary-monthly", async (
                [FromServices] IStatisticService statisticService,
                [AsParameters] DateRangeQuery requestCommand,
                CancellationToken cancellationToken) =>
            {
                var result = await statisticService.SummaryMonthlyAsync(requestCommand, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .Produces<ICollection<SummaryMonthlyDto>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<DateRangeQuery>()
            .WithDescription("Get total amount income and expense grouped by month within a date range.")
            .WithOpenApi();

        api.MapGet("/spent-per-category", async (
                [FromServices] IStatisticService statisticService,
                [AsParameters] DateRangeQuery requestCommand,
                CancellationToken cancellationToken) =>
            {
                var result = await statisticService.SpentPerCategoryAsync(requestCommand, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .Produces<ICollection<CategoryAmountDto>>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithValidation<DateRangeQuery>()
            .WithDescription("Get the total amount spent per category within a date range.")
            .WithOpenApi();
    }
}