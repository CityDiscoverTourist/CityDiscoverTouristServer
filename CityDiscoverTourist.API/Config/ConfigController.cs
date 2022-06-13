using System.Globalization;
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
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Config;

/// <summary>
///
/// </summary>
public static class ConfigController
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestService, QuestService>();
        services.AddScoped<IQuestTypeService, QuestTypeService>();
        services.AddScoped<IQuestItemTypeService, QuestItemTypeService>();
        services.AddScoped<IQuestItemService, QuestItemService>();
        services.AddScoped<IRewardService, RewardService>();
        services.AddScoped<IFacebookService, FacebookService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ICommissionService, CommissionService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<ICustomerAnswerService, CustomerAnswerService>();
        services.AddScoped<ICustomerQuestService, CustomerQuestService>();
        services.AddScoped<ICustomerTaskService, CustomerTaskService>();
        services.AddScoped<ICommissionService, CommissionService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ILocationTypeService, LocationTypeService>();
        services.AddScoped<IOwnerPaymentPeriodService, OwnerPaymentPeriodService>();
        services.AddScoped<IQuestOwnerService, QuestOwnerService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IOwnerPaymentService, OwnerPaymentService>();
        services.AddScoped<ISuggestionService, SuggestionService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IBlobService, BlobService>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    public static void SetupHelper(this IServiceCollection services)
    {
        services.AddScoped<ISortHelper<Quest>, SortHelper<Quest>>();
        services.AddScoped<ISortHelper<QuestType>, SortHelper<QuestType>>();
        services.AddScoped<ISortHelper<QuestItemType>, SortHelper<QuestItemType>>();
        services.AddScoped<ISortHelper<QuestItem>, SortHelper<QuestItem>>();
        services.AddScoped<ISortHelper<Reward>, SortHelper<Reward>>();
        services.AddScoped<ISortHelper<Commission>, SortHelper<Commission>>();
        services.AddScoped<ISortHelper<City>, SortHelper<City>>();
        services.AddScoped<ISortHelper<Area>, SortHelper<Area>>();
        services.AddScoped<ISortHelper<Location>, SortHelper<Location>>();
        services.AddScoped<ISortHelper<LocationType>, SortHelper<LocationType>>();
        services.AddScoped<ISortHelper<Note>, SortHelper<Note>>();
        services.AddScoped<ISortHelper<CustomerQuest>, SortHelper<CustomerQuest>>();
        services.AddScoped<ISortHelper<OwnerPaymentPeriod>, SortHelper<OwnerPaymentPeriod>>();
        services.AddScoped<ISortHelper<QuestOwner>, SortHelper<QuestOwner>>();
        services.AddScoped<ISortHelper<Wallet>, SortHelper<Wallet>>();
        services.AddScoped<ISortHelper<Transaction>, SortHelper<Transaction>>();
        services.AddScoped<ISortHelper<Payment>, SortHelper<Payment>>();
        services.AddScoped<ISortHelper<OwnerPayment>, SortHelper<OwnerPayment>>();
        services.AddScoped<ISortHelper<Suggestion>, SortHelper<Suggestion>>();
        services.AddScoped<ISortHelper<CustomerTask>, SortHelper<CustomerTask>>();
        services.AddScoped<ISortHelper<CustomerAnswer>, SortHelper<CustomerAnswer>>();
        services.AddScoped<ISortHelper<ApplicationUser>, SortHelper<ApplicationUser>>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    public static void SetupRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQuestRepository, QuestRepository>();
        services.AddScoped<IQuestTypeRepository, QuestTypeRepository>();
        services.AddScoped<IQuestItemRepository, QuestItemRepository>();
        services.AddScoped<IQuestItemTypeRepository, QuestItemTypeRepository>();
        services.AddScoped<ISuggestionRepository, SuggestionRepository>();
        services.AddScoped<IRewardRepository, RewardRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<ICustomerAnswerRepository, CustomerAnswerRepository>();
        services.AddScoped<ICustomerTaskRepository, CustomerTaskRepository>();
        services.AddScoped<ICustomerQuestRepository, CustomerQuestRepository>();
        services.AddScoped<ICommissionRepository, CommissionRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<ILocationTypeRepository, LocationTypeRepository>();
        services.AddScoped<IOwnerPaymentPeriodRepository, OwnerPaymentPeriodRepository>();
        services.AddScoped<IQuestOwnerRepository, QuestOwnerRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IOwnerPaymentRepository, OwnerPaymentRepository>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void SetupSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("EnableCORS",
                builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });

        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "City Tourist", Version = "v1" });
            var filePath = Path.Combine(AppContext.BaseDirectory, "CityDiscoverTourist.API.xml");
            c.IncludeXmlComments(filePath);
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
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
    }

    internal class LowercaseDashedParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object? value)
        {
            // Slugify value
            return Regex.Replace(value?.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2")
                .ToLower(new CultureInfo("en", false));
        }
    }
}