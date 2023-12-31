using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PwC.CRM.Share.CRMClients;
using PwC.CRM.Share.CRMClients.OData;

namespace PwC.CRM.Share.Extensions;

public static class CrmClientExtensions
{
    /// <summary>
    /// Adds CRM clients to the specified IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddCRMClients(this IServiceCollection services, IConfiguration configuration)
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

            clientFactory.AddODataHttpClient(clientType, RegistODataHttpClient(configuration, clientType));
        }

        services.AddSingleton<ICRMClientFactory>(clientFactory);
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

