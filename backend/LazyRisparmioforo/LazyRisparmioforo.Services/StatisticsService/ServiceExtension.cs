using Microsoft.Extensions.DependencyInjection;

namespace StatisticsService;

public static class ServiceExtension
{
    public static void AddStatisticService(this IServiceCollection services)
    {
        services.AddTransient<IStatisticService, StatisticService>();
    }
}