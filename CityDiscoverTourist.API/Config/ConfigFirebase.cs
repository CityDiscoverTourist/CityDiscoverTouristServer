using System.Text;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CityDiscoverTourist.API.Config;

/// <summary>
/// </summary>
public static class ConfigFirebase
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="webHostEnvironment"></param>
    public static void SetupFirebaseAuth( this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            opt.Password.RequireUppercase = false;
            opt.Password.RequireDigit = false;
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Lockout.AllowedForNewUsers = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            opt.Lockout.MaxFailedAccessAttempts = 3;
            opt.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        if (webHostEnvironment.IsDevelopment())
        {
            var fireBaseCredential = new FirestoreCredentialInitializer(configuration);
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            FirebaseApp.Create(new AppOptions
            {
                Credential =
                    GoogleCredential.FromJson(JsonConvert.SerializeObject(fireBaseCredential, serializerSettings))
            });
        }
        else
        {
            var fireBaseCredential = configuration.GetSection("Firebase").Value;
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(fireBaseCredential)
            });
        }


        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });
    }
}