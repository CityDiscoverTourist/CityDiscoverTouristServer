using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace CityDiscoverTourist.Business.HealthCheck;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisHealthCheck(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new ())
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();
            database.StringGet("/api/healthcheck");
            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "Redis is healthy"));
        }
        catch (Exception e)
        {
            return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, e.Message));
        }
    }
}