using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Business.Settings;

namespace CityDiscoverTourist.API.Config;

public static class ThirdPartyConfig
{
    public static void SetupThirdParty(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();

        var googleConfig = configuration.GetSection("Googleapis").Get<GoogleApiSetting>() ?? new GoogleApiSetting();
        services.AddSingleton(googleConfig);
    }
}