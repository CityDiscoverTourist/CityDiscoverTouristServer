
using CityDiscoverTourist.Business.Settings;

namespace CityDiscoverTourist.API.Config;

public static class ConfigApplicationInsight
{
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