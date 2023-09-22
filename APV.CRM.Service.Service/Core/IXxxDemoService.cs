using APV.CRM.Service.Service.Dto.Request;
using CrmModels;
using PwC.Crm.Share.CommonCode;

namespace APV.CRM.Service.Service.Core
{
    public interface IXxxDemoService : IBaseService, IDependency
    {
        Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter);
    }
}