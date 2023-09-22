using APV.CRM.Service.Service.Dto.Request;
using PwC.Crm.Share.Log.Serilogs.Models;
using PwC.Crm.Share.CommonCode;

namespace APV.CRM.Service.Service.Core.LogRepositorys
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMySqlLogRepository : IDependency
    {
        Task<List<LoggerSimpleDO>> QueryLogsAsync(LogRequestDto request);
    }
}
