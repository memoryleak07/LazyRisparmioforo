using LazyRisparmioforo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LazyRisparmioforo.Infrastructure;

public static class ServiceExtension
{
    public static void AddInfrastructuresServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ApplicationDbContextInitializer>();
    }
}