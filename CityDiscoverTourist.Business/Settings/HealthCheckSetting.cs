namespace CityDiscoverTourist.Business.HealthCheck;

public class HealthCheckSetting
{
    public string? Status { get; set; }
    public IEnumerable<HealthCheck> Checks { get; set; }
    public TimeSpan Duration { get; set; }
}