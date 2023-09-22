using PwC.Crm.Share.CommonCode;
using PwC.Crm.Share.CRMClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PwcNetCore;

namespace APV.CRM.Service.Service.Core
{
    public interface ICommonInjectionObject : IDependency
    {

        public IConfiguration Configuration { get; }
        public ICRMClientFactory CrmClientFactory { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
    }
}
