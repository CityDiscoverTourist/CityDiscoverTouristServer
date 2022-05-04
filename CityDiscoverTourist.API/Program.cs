using Amazon;
using CityDiscoverTourist.API.Config;
using CityDiscoverTourist.Business.Data;


var builder = WebApplication.CreateBuilder(args);

/*var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Configuration.AddSecretsManager(region: RegionEndpoint.APSoutheast1, configurator: options =>
{
    //options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
    options.KeyGenerator = (secret, name) => name.Replace("__", ":");
});*/


// Add services to the container.
builder.Services.SetupDatabase(builder.Configuration);
builder.Services.SetupFirebaseAuth(builder.Configuration);
builder.Services.SetupRepositories();
builder.Services.SetupHelper();
builder.Services.SetupServices();
builder.Services.SetupThirdParty(builder.Configuration);
builder.Services.SetupSwagger(builder.Configuration);


builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();