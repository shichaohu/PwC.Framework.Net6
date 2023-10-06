using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PwC.CRM.Share.CommonCode;
using PwC.CRM.Share.CRMClients;

namespace PwC.CRM.Service.Core
{
    public interface ICommonInjectionObject : IDependency
    {

        public IConfiguration Configuration { get; }
        public ICRMClientFactory CrmClientFactory { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
    }
}
