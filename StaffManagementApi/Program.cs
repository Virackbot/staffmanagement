using Microsoft.EntityFrameworkCore;
using Domain.OutfaceModels;
using Domain.Interfaces;
using App.Db;
using Microsoft.OpenApi.Models;
using Sentry;
using App.Helpers.MiddleWares;
using App.Logics;
using App.Services;
using Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using StaffManagement.API.Helpers;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Bind AppSetting from configuration
var envConf = builder.Configuration.GetSection("App");
var appSetting = new AppSetting();
envConf.Bind(appSetting);
builder.Services.Configure<AppSetting>(envConf);

// Sentry
if (appSetting.EnableSentry)
{
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = appSetting.SentrySettings.Dsn;
        o.Debug = appSetting.SentrySettings.Debug;
        o.TracesSampleRate = appSetting.SentrySettings.TracesSampleRate;
        o.Environment = appSetting.SentrySettings.Environment;
        o.Release = $"{appSetting.ApiVersion}";

        o.MaxRequestBodySize = Sentry.Extensibility.RequestSize.Always;
        o.IncludeActivityData = appSetting.SentrySettings.IncludeActivityData;
        o.AttachStacktrace = appSetting.SentrySettings.AttachStacktrace;
    });
}

// Register DbContext
builder.Services.AddDbContext<StaffManagementDbContext>(options =>
    options.UseNpgsql(appSetting.Db.PrimaryConnection));
ExcelPackage.License.SetNonCommercialPersonal("TheBotz");
// Register business logic and services
builder.Services.AddScoped<StaffLogic>();
// builder.Services.AddScoped<ExportService>(); // Removed because ExportService is static
builder.Services.AddScoped<IHttpContextScope, StaffManagementHttpContextScope>();

// Add controllers with Newtonsoft JSON to avoid .NET 9.0 PipeWriter issues
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    options.SerializerSettings.Converters.Add(new DateOnlyNewtonsoftConverter());
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
});

// Add Swagger/ OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Staff Management API", Version = "v1" });
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Add ErrorHandlingMiddleware to capture all unhandled exceptions
app.UseMiddleware<ErrorHandlingMiddleware>();

// Enable Swagger middleware
if (appSetting.EnableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Staff Management API v1");
        setup.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Initialize database on startup
if (appSetting.AutoMigration)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<StaffManagementDbContext>();
    await context.Database.EnsureCreatedAsync();
}

await app.RunAsync();

// Make Program class accessible for testing
public partial class Program
{
    protected Program() { }
}