using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.API.Config;

public static class ConfigDatabase
{
    public static void SetupDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? string.Empty, o =>
                o.EnableRetryOnFailure()));
    }
}