using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.Log.Serilogs.Models;
using PwC.CRM.Share.CommonCode;

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
