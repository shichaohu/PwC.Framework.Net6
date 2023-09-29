using PwC.CRM.Service.Dto.Request;
using CrmModels;
using PwC.Crm.Share.CommonCode;

namespace PwC.CRM.Service.Core
{
    public interface IXxxDemoService : IBaseService, IDependency
    {
        Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter);
    }
}