using LazyRisparmioforo.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = await builder
    .ConfigureServices(builder.Configuration)
    .ConfigurePipelineAsync();

await app.RunAsync();