using LazyRisparmioforo.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    Path.Combine(AppContext.BaseDirectory, "categories.json"), 
    optional: false, 
    reloadOnChange: true);

var app = await builder
    .ConfigureServices(builder.Configuration)
    .ConfigurePipelineAsync();

await app.RunAsync();