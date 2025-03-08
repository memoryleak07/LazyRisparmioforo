using System.Net.Http.Json;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Infrastructure.Data;
using LazyRisparmioforo.Shared.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CategoryService;

public class CategoryService(
    ApplicationDbContext dbContext,
    HttpClient httpClient,
    ILogger<CategoryService> logger)
    : ICategoryService
{
    private const string PostPredictUrl = "http://localhost:80/api/predict";
    private const string PostPredictBatchUrl = "http://localhost:80/api/predict-batch";

    public async Task<ICollection<Category>> AllAsync(CancellationToken cancellationToken)
        => await dbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
    
    public Task<Result<CategoryPredictResult>> PredictAsync(CategoryPredictCommand command, CancellationToken cancellationToken) 
        => PredictAsync<CategoryPredictResult>(
            PostPredictUrl, new { input = command.Input }, cancellationToken);

    public Task<Result<ICollection<CategoryPredictResult>>> PredictBatchAsync(CategoryPredictBatchCommand command, CancellationToken cancellationToken) 
        => PredictAsync<ICollection<CategoryPredictResult>>(
            PostPredictBatchUrl, new { input = command.Input }, cancellationToken);
    
    private async Task<Result<T>> PredictAsync<T>(string url, object request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(url, request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result.Failure<T>(Errors.ClientError);

        var prediction = await response.Content.ReadFromJsonAsync<T>(cancellationToken);

        return prediction is null
            ? Result.Failure<T>(Errors.CouldNotDeserialize)
            : Result.Success(prediction);
    }
}