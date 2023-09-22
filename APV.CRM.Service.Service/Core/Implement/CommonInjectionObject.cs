using PwC.Crm.Share.CRMClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PwcNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APV.CRM.Service.Service.Core.Implement
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
