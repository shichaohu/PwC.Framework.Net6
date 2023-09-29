using PwC.CRM.Service.Dto.Request;
using PwC.Crm.Share.Log.Serilogs.Models;
using PwC.Crm.Share.CommonCode;

namespace PwC.CRM.Service.Core.LogRepositorys
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMySqlLogRepository : IDependency
    {
        Task<List<LoggerSimpleDO>> QueryLogsAsync(LogRequestDto request);
    }
}
