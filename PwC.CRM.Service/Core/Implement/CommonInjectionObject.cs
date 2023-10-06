using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PwC.CRM.Share.CRMClients;

namespace PwC.CRM.Service.Core.Implement
{
    public class CommonInjectionObject : ICommonInjectionObject
    {

        public IConfiguration Configuration { get; }
        public ICRMClientFactory CrmClientFactory { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public CommonInjectionObject(IConfiguration configuration, ICRMClientFactory crmClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            CrmClientFactory = crmClientFactory;
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
