using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Python script settings
var pythonAppSettings = builder.Configuration.GetSection("PythonAppSettings");
#pragma warning disable ASPIREHOSTINGPYTHON001
var pythonApp = builder.AddPythonApp(
        name: "api-categorizer",
        projectDirectory: Path.GetFullPath(pythonAppSettings["ProjectDirectory"]!, builder.Environment.ContentRootPath),
        scriptPath: pythonAppSettings["Path"]!,
        virtualEnvironmentPath: Path.GetFullPath(pythonAppSettings["VirtualEnvPath"]!, builder.Environment.ContentRootPath))
    .WithHttpEndpoint(port: int.Parse(pythonAppSettings["Port"]!), env: "PORT")
    .WithExternalHttpEndpoints()
    .WithOtlpExporter()
    .PublishAsDockerFile();
#pragma warning restore ASPIREHOSTINGPYTHON001

if (builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
{
    pythonApp.WithEnvironment("DEBUG", "True");
}

var apiService = builder.AddProject<Projects.LazyRisparmioforo_Api>(
        name: "api-service")
    .WaitFor(pythonApp);

// builder.AddNpmApp(
//         name:"angular-ui",
//         workingDirectory: "../../frontend/LazyRisparmioforo.UI")
//     .WithReference(apiService)
//     .WaitFor(apiService)
//     .WithHttpEndpoint(port: 80, targetPort: 4200)
//     .WithExternalHttpEndpoints()
//     .PublishAsDockerFile();

await builder.Build().RunAsync();