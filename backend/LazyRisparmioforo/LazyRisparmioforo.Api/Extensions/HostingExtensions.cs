using CategoryService;
using LazyRisparmioforo.Api.Endpoints;
using LazyRisparmioforo.Api.Middleware;
using LazyRisparmioforo.Infrastructure;
using LazyRisparmioforo.Infrastructure.Data;
using TransactionService;
using FluentValidation;
using ImportCsvService;
using LazyRisparmioforo.Domain.Validators;
using StatisticsService;

namespace LazyRisparmioforo.Api.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddInfrastructuresServices(configuration);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddProblemDetails(); 
        builder.Services.AddHttpClient();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAntiforgery();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddCategoryService();

        builder.Services.AddTransactionService();
        builder.Services.AddValidatorsFromAssemblyContaining<TransactionValidators>();
        
        builder.Services.AddImportCsvService();
        builder.Services.AddValidatorsFromAssemblyContaining<UploadFileViewModelValidator>();
        
        builder.Services.AddStatisticService();
        builder.Services.AddValidatorsFromAssemblyContaining<StatisticValidators>();

        return builder.Build();
    }
    
    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app)
    {
        using var loggerFactory = LoggerFactory.Create(builder => { });
        
        await using var scope = app.Services.CreateAsyncScope();
        
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitializeAsync();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAntiforgery();
        
        app.UseMiddleware<GlobalExceptionHandler>();
        
        app.MapTransactionEndpoints();
        app.MapImportEndpoints();
        app.MapCategoryEndpoints();
        app.MapStatisticEndpoints();
        
        return app;
    }
}