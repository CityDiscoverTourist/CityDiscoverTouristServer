using System.Text.RegularExpressions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.IRepositories.Repositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.API.Config;

/// <summary>
///
/// </summary>
public static class ConfigController
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestService, QuestService>();
        services.AddScoped<IQuestTypeService, QuestTypeService>();
        services.AddScoped<ITaskTypeService, TaskTypeService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IRewardService, RewardService>();
    }

    public static void SetupHelper(this IServiceCollection services)
    {
        services.AddScoped<ISortHelper<Quest>, SortHelper<Quest>>();
        services.AddScoped<IDataShaper<Quest>, DataShaper<Quest>>();
        services.AddScoped<ISortHelper<QuestType>, SortHelper<QuestType>>();
        services.AddScoped<IDataShaper<QuestType>, DataShaper<QuestType>>();
        services.AddScoped<ISortHelper<TaskType>, SortHelper<TaskType>>();
        services.AddScoped<IDataShaper<TaskType>, DataShaper<TaskType>>();
        services.AddScoped<ISortHelper<Task>, SortHelper<Task>>();
        services.AddScoped<IDataShaper<Task>, DataShaper<Task>>();
        services.AddScoped<ISortHelper<Reward>, SortHelper<Reward>>();
        services.AddScoped<IDataShaper<Reward>, DataShaper<Reward>>();
    }

    public static void SetupRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQuestRepository, QuestRepository>();
        services.AddScoped<IQuestTypeRepository, QuestTypeRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
        services.AddScoped<ISuggestionRepository, SuggestionRepository>();
        services.AddScoped<IRewardRepository, RewardRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IExperienceRepository, ExperienceRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
    }

    public static void SetupSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "City Tourist", Version = "v1" });
        });

        services.AddSwaggerGen(setup =>
        {
            // Include 'SecurityScheme' to use JWT Authentication
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

        services.AddApiVersioning(options =>
        {
            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseDashedParameterTransformer()));
        }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }
        );
    }

    internal class LowercaseDashedParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object? value)
        {
            // Slugify value
            return Regex.Replace(value?.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}