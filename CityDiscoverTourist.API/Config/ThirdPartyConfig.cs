using Azure.Storage.Blobs;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Business.Settings;

namespace CityDiscoverTourist.API.Config;

public static class ThirdPartyConfig
{
    public static void SetupThirdParty(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();

        var googleConfig = configuration.GetSection("GoongApi").Get<GoongApiSetting>() ?? new GoongApiSetting();
        services.AddSingleton(googleConfig);

        services.AddSingleton(x => new BlobServiceClient(configuration.GetSection("AzureStorage:ConnectionString").Value));
        services.AddSingleton<IBlobService, BlobService>();

    }
}