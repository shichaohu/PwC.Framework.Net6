using PwC.Crm.Share.CRMClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using PwcNetCore;
using System.Configuration;

namespace PwC.Crm.Share.Extensions;

public static class CrmClientExtensions
{

    public static void UseCRMClients(this IServiceCollection services, IConfiguration configuration)
    {
        CRMClientFactory clientFactory = new();
        clientFactory.AddCRMClient(CRMClientTypeEnum.Default, RegistCRMClient(configuration, CRMClientTypeEnum.Default));
        clientFactory.AddCRMClient(CRMClientTypeEnum.HK, RegistCRMClient(configuration, CRMClientTypeEnum.HK));

        clientFactory.AddCRMConnectionString(CRMClientTypeEnum.Default, configuration.GetSection("Crm:Default:connectionString").Value);
        clientFactory.AddCRMConnectionString(CRMClientTypeEnum.HK, configuration.GetSection("Crm:HK:connectionString").Value);

        services.AddSingleton<ICRMClientFactory>(clientFactory);
    }
    private static ICrequest RegistCRMClient(IConfiguration configuration, CRMClientTypeEnum clientType)
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
}

