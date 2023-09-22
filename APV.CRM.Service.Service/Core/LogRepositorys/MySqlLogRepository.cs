using APV.CRM.Service.Service.Core.DapperRepositorys;
using APV.CRM.Service.Service.Dto.Request;
using PwC.Crm.Share.Log.Serilogs.Models;
using PwC.Crm.Share.Log.Serilogs.Util;
using Microsoft.Extensions.Configuration;

namespace APV.CRM.Service.Service.Core.LogRepositorys
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlLogRepository : DapperRepository, IMySqlLogRepository
    {
        public MySqlLogRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<List<LoggerSimpleDO>> QueryLogsAsync(LogRequestDto request)
        {
            string tableName = SerilogsLogUtil.GetPracticalTableName(request.TimeStart ?? DateTime.Now, LogTableName);
            var sqlStr = @$" select ID,HttpHost, HttpRemoteAddress,HttpXForwardedFor,HttpPath, HttpRequestId, SourceContext, Timestamp, Level, Message, Exception
                            from {tableName} where 1=1";

            if (!string.IsNullOrWhiteSpace(request.HttpRequestId))
            {
                sqlStr += $" and HttpRequestId = '{request.HttpRequestId}' ";
            }
            if (!string.IsNullOrWhiteSpace(request.HttpHost))
            {
                sqlStr += $" and HttpHost like '%{request.HttpHost}%' ";
            }
            if (!string.IsNullOrWhiteSpace(request.HttpPath))
            {
                sqlStr += $" and HttpPath like '%{request.HttpPath}%' ";
            }
            if (!string.IsNullOrWhiteSpace(request.Message))
            {
                sqlStr += $" and Message like '%{request.Message}%' ";
            }
            if (!string.IsNullOrWhiteSpace(request.SourceContext))
            {
                sqlStr += $" and SourceContext like '%{request.SourceContext}%' ";
            }
            if (request.Level.HasValue)
            {
                sqlStr += $" and Level = '{request.Level.Value}' ";
            }
            if (request.TimeStart.HasValue)
            {
                sqlStr += $" and `Timestamp` >= '{request.TimeStart}' ";
            }
            if (request.TimeEnd.HasValue)
            {
                sqlStr += $" and `Timestamp` <= '{request.TimeEnd}' ";
            }
            if (request.Limit <= 0)
            {
                request.Limit = 50;
            }
            sqlStr += $" order by `Timestamp` desc limit {request.Limit} ";

            var res = await QueryListAsync<LoggerSimpleDO>(sqlStr);
            return res;
        }
    }
}
