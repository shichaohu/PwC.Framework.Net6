using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.CommonCode;
using PwC.CRM.Models.Table;

namespace PwC.CRM.Service.Core
{
    public interface IDemoService : IBaseService, IDependency
    {
        Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter);
    }
}