using CityDiscoverTourist.Business.HealthCheck;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.API.Config;

public static class HealthCheckConfig
{
    public static void SetUpHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        //.AddCheck<RedisHealthCheck>("redis");
    }
}