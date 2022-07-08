namespace CityDiscoverTourist.API.Config;

public static class SignalRConfig
{
    public static void SetUpSignalR(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        if (envName == "Development")
        {
            services.AddSignalR();
        }
        else
        {
            var signalRConfig = configuration.GetSection("SignalR:ConnectionString").Value;

            services.AddSignalR(op => { op.EnableDetailedErrors = true; }).AddAzureSignalR(options =>
            {
                options.ConnectionString = signalRConfig;
                options.ConnectionCount = 10;
            });
        }
    }
}