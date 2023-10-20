using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PwC.CRM.Api.HttpClients;
using PwC.CRM.Api.Swagger;
using PwC.CRM.Share.Authentication;
using PwC.CRM.Share.Extensions;
using PwC.CRM.Share.Handlers;
using PwC.CRM.Share.Log.Serilogs;
using PwC.CRM.Share.Util;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.UseFileUpload(builder.WebHost);

//enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.Configure<TelemetryConfiguration>(x =>
    x.DisableTelemetry = bool.TryParse(builder.Configuration["ApplicationInsights:DisableTelemetry"], out bool disableTelemetry) && disableTelemetry
);

builder.Services.AddControllers(option =>
{
    option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    option.Filters.Add(typeof(GlobalExceptionHandler));
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddCRMClients(builder.Configuration);
builder.Services.AddAutoDependency("PwC.CRM.Service");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<LocalCache>();
builder.Services.AddModelStateVrify();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
    //.WithExposedHeaders("X-Pagination"));
});

builder.Services.AddSwaggerDoc(builder.Configuration);
builder.Services.AddCustomerHttpClient(builder.Configuration);

builder.Host.AddLogStrategy(builder.Logging, builder.Services, builder.Configuration);


var app = builder.Build();
app.UseLog(app.Environment);
app.UseCors("CorsPolicy");
app.UseSwaggerUi(builder.Configuration, app.Environment);

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();
app.Logger.Log(LogLevel.Information, $"App Environment:{JsonConvert.SerializeObject(app.Environment)}");

app.Run();
