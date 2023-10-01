using PwC.CRM.Share.CommonCode;
using PwC.CRM.Share.CRMClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PwcNetCore;

namespace PwC.CRM.Service.Core
{
    public interface ICommonInjectionObject : IDependency
    {

        public IConfiguration Configuration { get; }
        public ICRMClientFactory CrmClientFactory { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
    }
}
