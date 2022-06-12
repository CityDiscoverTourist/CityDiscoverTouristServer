using CityDiscoverTourist.API.Config;
using CityDiscoverTourist.Business.Data;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using CityDiscoverTourist.API.AzureHelper;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;


if (env == "Production")
{
    var vaultName = builder.Configuration["KeyVault:Vault"];
    if (!string.IsNullOrEmpty(vaultName))
    {
        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        var keyVaultClient =
            new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        builder.Configuration.AddAzureKeyVault($"https://{vaultName}.vault.azure.net/", keyVaultClient,
            new PrefixKeyVault("CityDiscoverTouristAPI"));
    }
}

var con = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
#pragma warning disable CS0618
    .WriteTo.ApplicationInsights(TelemetryConfiguration.Active.InstrumentationKey, TelemetryConverter.Traces)
#pragma warning restore CS0618
    .CreateLogger();

try
{
    Log.Information("Starting up");
    // Full setup of serilog. We read log settings from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.MSSqlServer(con,
             new MSSqlServerSinkOptions { TableName = "UserLogs"}, restrictedToMinimumLevel: LogEventLevel.Information)
        .Enrich.FromLogContext());


    const string managedNetworkingAppContextSwitch = "Switch.Microsoft.Data.SqlClient.UseManagedNetworkingOnWindows";
    AppContext.SetSwitch(managedNetworkingAppContextSwitch, true);
    // Add services to the container.
    builder.Services.SetupDatabase(builder.Configuration);
    builder.Services.SetupFirebaseAuth(builder.Configuration, builder.Environment);
    builder.Services.SetupRepositories();
    builder.Services.SetupHelper();
    builder.Services.SetupServices();
    builder.Services.SetupThirdParty(builder.Configuration);
    builder.Services.SetupSwagger(builder.Configuration);
    builder.Services.SetUpCache(builder.Configuration);
    builder.Services.SetUpHealthCheck(builder.Configuration);
    builder.Services.SetUpApplicationInsight(builder.Configuration);

    builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

    var app = builder.Build();
    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging(_ =>
    {
    }); // We want to log all HTTP requests

    // Configure the HTTP request pipeline.
    app.UseForwardedHeaders();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CityDiscoverTourist API Production"));
    }

    app.UseHealthChecks("/api/healthcheck", new HealthCheckOptions
    {
        ResponseWriter  = async (context, report) =>
        {
            context.Response.ContentType = "application/json";

            var response = new HealthCheckResponse
            {
                Status = report.Status.ToString(),
                Checks = report.Entries.Select(x => new HealthCheck
                {
                    Component = x.Key,
                    Status = x.Value.Status.ToString(),
                    Description = x.Value.Description
                }),
                Duration = report.TotalDuration
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    });
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.HandlerExceptionProduction(app.Environment.IsDevelopment());
    app.UseHttpsRedirection();
    app.UseHsts();
    app.UseCors("EnableCORS");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}