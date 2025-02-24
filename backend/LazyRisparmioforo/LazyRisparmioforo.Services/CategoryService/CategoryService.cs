using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Domain.Shared;
using LazyRisparmioforo.Infrastructure.Data;
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

    public async Task<ICollection<Category>> AllAsync(CancellationToken cancellationToken)
        => await dbContext.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    
    public async Task<Result<CategoryPredictResult>> PredictAsync(CategoryPredictCommand command, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            requestUri: PostPredictUrl, 
            value: new { transaction_text = command.Input }, 
            cancellationToken: cancellationToken);
        
        if (!response.IsSuccessStatusCode)
            return Result.Failure<CategoryPredictResult>(Errors.ClientError);
        
        var prediction =  await response.Content.ReadFromJsonAsync<CategoryPredictResult?>(cancellationToken);
        
        return prediction is null 
            ? Result.Failure<CategoryPredictResult>(Errors.CouldNotDeserialize)
            : Result.Success(prediction);
    }
}