using CityDiscoverTourist.API.Config;
using CityDiscoverTourist.Business.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.SetupDatabase(builder.Configuration);
builder.Services.SetupFirebaseAuth(builder.Configuration);
builder.Services.SetupRepositories(builder.Configuration);
builder.Services.SetupServices(builder.Configuration);
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