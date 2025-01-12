using Risparmioforo.Api.Endpoints;
using Risparmioforo.Api.Middleware;
using Risparmioforo.Infrastructure;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Services.CategoryService;
using Risparmioforo.Services.ImportFileService;
using Risparmioforo.Services.TransactionService;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Api.Extensions;

public static class HostingExtension
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddInfrastructuresServices(appSettings);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAntiforgery();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        
        builder.Services.AddTransactionService();
        builder.Services.AddImportFileService();
        builder.Services.AddCategoryService();
        
        return builder.Build();
    }
    
    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appSettings)
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
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.MapTransactionEndpoints();
        app.MapImportFileEndpoints();
        app.MapCategoryEndpoints();
        
        return app;
    }
}