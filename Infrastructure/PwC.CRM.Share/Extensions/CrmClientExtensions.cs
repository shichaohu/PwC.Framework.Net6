using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PwC.Crm.Share.CRMClients;
using PwC.Crm.Share.CRMClients.OData;
using PwcNetCore;

namespace PwC.Crm.Share.Extensions;

public static class CrmClientExtensions
{

    public static void UseCRMClients(this IServiceCollection services, IConfiguration configuration)
    {
        CRMClientFactory clientFactory = new();

        string[] nameList = Enum.GetNames(typeof(CRMClientTypeEnum));
        foreach (var name in nameList)
        {
            var clientType = Enum.Parse<CRMClientTypeEnum>(name);
            string connectionString = configuration.GetSection($"Crm:{name}:connectionString")?.Value;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                clientFactory.AddCRMConnectionString(clientType, connectionString);
            }

            clientFactory.AddCrequest(clientType, RegistRequest(configuration, clientType));
            clientFactory.AddODataHttpClient(clientType, RegistODataHttpClient(configuration, clientType));
        }

        services.AddSingleton<ICRMClientFactory>(clientFactory);
    }
    private static ICrequest RegistRequest(IConfiguration configuration, CRMClientTypeEnum clientType)
    {
        string configPrefix = $"CRM:{clientType}";

        var resourceUrl = configuration.GetSection($"{configPrefix}:resourceUrl").Value;
        if (!resourceUrl.EndsWith("/"))
        {
            resourceUrl += "/";
        }
        return new CRequest(resourceUrl,
                configuration.GetSection($"{configPrefix}:clientId").Value,
                configuration.GetSection($"{configPrefix}:clientSecret").Value,
                configuration.GetSection($"{configPrefix}:tenantId").Value,
                configuration.GetSection($"{configPrefix}:tokenUrl").Value
                );
    }
    private static IODataHttpClient RegistODataHttpClient(IConfiguration configuration, CRMClientTypeEnum clientType)
    {
        string configPrefix = $"CRM:{clientType}";

        var resourceUrl = configuration.GetSection($"{configPrefix}:resourceUrl").Value;
        if (!resourceUrl.EndsWith("/"))
        {
            resourceUrl += "/";
        }
        return new ODataHttpClient(resourceUrl,
                configuration.GetSection($"{configPrefix}:clientId").Value,
                configuration.GetSection($"{configPrefix}:clientSecret").Value,
                configuration.GetSection($"{configPrefix}:tenantId").Value,
                configuration.GetSection($"{configPrefix}:tokenUrl").Value
                );
    }
}

