using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.API.Config;

/// <summary>
/// </summary>
public static class HealthCheckConfig
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void SetUpHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        //.AddCheck<RedisHealthCheck>("redis");
    }
}