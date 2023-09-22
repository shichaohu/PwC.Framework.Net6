using APV.CRM.Service.Api.HttpClients;
using APV.CRM.Service.Api.Swagger;
using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PwC.Crm.Share.Authentication;
using PwC.Crm.Share.CRMClients;
using PwC.Crm.Share.Extensions;
using PwC.Crm.Share.Handlers;
using PwC.Crm.Share.Log.Serilogs;
using PwC.Crm.Share.PwcNetCore;
using PwC.Crm.Share.Util;
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
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

#region ��ӷ�������  
builder.Services.AddSingleton<ICRMClient, CRMClient>();

builder.Services.UseCRMClients(builder.Configuration);
#endregion

// �Զ�ע��APV.CRM.Service.Service.Core��ʵ��IDependency�Ľӿ�
builder.Services.UseAutoDependency("APV.CRM.Service");
builder.Services.AddSingleton<LocalCachelper>();
builder.Services.AddSingleton(x => new BlobContainerClient(builder.Configuration.GetSection("AzureBlob:connString").Value, "files"));
//builder.Services.AddApiVersioning();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.UseModelStatevVrify();

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

builder.Host.UseLogStrategy(builder.Logging, builder.Services, builder.Configuration);


var app = builder.Build();
app.AddLogStrategy(app.Environment);
app.UseCors("CorsPolicy");

//�����м����������Swagger��ΪJSON�ս��
app.UseSwaggerUi(builder.Configuration, app.Environment);

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();
app.Logger.Log(LogLevel.Information, $"App Environment:{JsonConvert.SerializeObject(app.Environment)}");

app.Run();
