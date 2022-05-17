using Amazon;
using Amazon.Runtime;
using CityDiscoverTourist.API.Config;
using CityDiscoverTourist.Business.Data;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    // Full setup of serilog. We read log settings from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    var env = builder.Environment.EnvironmentName;
    var appName = builder.Environment.ApplicationName;
    var credentials = new StoredProfileAWSCredentials("production");

    builder.Configuration.AddSecretsManager(
        credentials: credentials,
        region: RegionEndpoint.APSoutheast1, configurator: options =>
        {
            //arn:aws:secretsmanager:ap-southeast-1:958841795550:secret:Production_CityDiscoverTourist.API_ConnectionStrings__DefaultConnection-65jWxM
            options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
            options.KeyGenerator = (_, s) => s.Replace($"{env}_{appName}_", string.Empty).Replace("__", ":");
        });

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