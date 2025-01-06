using Microsoft.AspNetCore.Mvc;

namespace Risparmioforo.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate requestDelegate, 
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured: {Message}", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}