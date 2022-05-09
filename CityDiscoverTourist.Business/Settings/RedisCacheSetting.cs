namespace CityDiscoverTourist.Business.Settings;

public class RedisCacheSetting
{
    public bool Enabled { get; set; }
    public string ConnectionString { get; set; } = null!;
}