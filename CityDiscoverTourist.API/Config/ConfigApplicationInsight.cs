using CityDiscoverTourist.Business.Settings;

namespace CityDiscoverTourist.API.Config;

/// <summary>
/// </summary>
public static class ConfigApplicationInsight
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void SetUpApplicationInsight(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = new ApplicationInsightSettings();
        configuration.GetSection("ApplicationInsights").Bind(appSettings);
        services.AddSingleton(appSettings);

        services.AddApplicationInsightsTelemetry(o =>
        {
            o.InstrumentationKey = appSettings.InstrumentationKey;
            o.EnableAdaptiveSampling = false;
            o.EnableActiveTelemetryConfigurationSetup = true;
        });
    }
}