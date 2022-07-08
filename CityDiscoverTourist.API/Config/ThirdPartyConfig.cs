using Azure.Storage.Blobs;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Business.Settings;
using Hangfire;

namespace CityDiscoverTourist.API.Config;

/// <summary>
/// </summary>
public static class ThirdPartyConfig
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void SetupThirdParty(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        if (envName == "Production")
        {
            var notifyConfig = configuration.GetSection("FcmNotification").Value;
            var notify = new NotificationSetting
            {
                SenderId = notifyConfig.Split(',')[0],
                ServerKey = notifyConfig.Split(',')[1]
            };
            services.AddSingleton(notify);
        }
        else
        {
            var notifyConfig = configuration.GetSection("FcmNotification").Get<NotificationSetting>() ??
                               new NotificationSetting();
            services.AddSingleton(notifyConfig);
        }

        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>() ??
                          new EmailConfiguration();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();

        var googleConfig = configuration.GetSection("GoongApi").Get<GoongApiSetting>() ?? new GoongApiSetting();
        services.AddSingleton(googleConfig);

        var momoConfig = configuration.GetSection("Momo").Get<MomoSetting>() ?? new MomoSetting();
        services.AddSingleton(momoConfig);

        services.AddSingleton(_ =>
            new BlobServiceClient(configuration.GetSection("AzureStorage:ConnectionString").Value));
        services.AddSingleton<IBlobService, BlobService>();

        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();
    }
}