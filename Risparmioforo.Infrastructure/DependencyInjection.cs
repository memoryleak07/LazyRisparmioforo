using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructuresServices(this IServiceCollection services, AppSettings configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.ConnectionStrings.DefaultConnection));

        services.AddScoped<ApplicationDbContextInitializer>();
    }
}