using Risparmioforo.Api.Extensions;
using Risparmioforo.Shared.Base;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.Get<AppSettings>()
                  ?? throw new ArgumentNullException(nameof(AppSettings));

builder.Services.AddSingleton(appSettings);

var app = await builder
    .ConfigureServices(appSettings)
    .ConfigurePipelineAsync(appSettings);

await app.RunAsync();