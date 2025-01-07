using Risparmioforo.Api.Endpoints;
using Risparmioforo.Api.Middleware;
using Risparmioforo.Infrastructure;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Services.TransactionService;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Api;

public static class HostingExtension
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddInfrastructuresServices(appSettings);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        
        builder.Services.AddScoped<ITransactionService, TransactionService>();

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
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapTransactionEndpoints();
        
        return app;
    }
}