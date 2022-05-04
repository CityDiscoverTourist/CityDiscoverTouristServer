using CityDiscoverTourist.Business.Helper.EmailHelper;

namespace CityDiscoverTourist.API.Config;

public static class ThirdPartyConfig
{
    public static void SetupThirdParty(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();
    }
}