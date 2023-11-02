using PwC.CRM.Service.Core.DapperRepositorys;
using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.Log.Serilogs.Models;
using PwC.CRM.Share.Log.Serilogs.Util;
using Microsoft.Extensions.Configuration;

namespace PwC.CRM.Service.Core.LogRepositorys
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlLogRepository : DapperRepository, IMySqlLogRepository
    {
        public MySqlLogRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<List<LoggerSimpleDO>> QueryLogsAsync(LogRequestDto request)
        {
            List<string> tableNames = SerilogsLogUtil.GetPracticalTableNameList(
                request.TimeStart ?? DateTime.Now,
                request.TimeEnd ?? DateTime.Now,
                LogTableName
                );

            var sqlModel = @$" select ID,HttpHost, HttpRemoteAddress,HttpXForwardedFor,HttpPath, HttpRequestId, SourceContext, Timestamp, Level, Message, Exception
                            from tableName_Placeholder where 1=1";

            if (!string.IsNullOrWhiteSpace(request.HttpRequestId))
            {
                sqlModel += $" and HttpRequestId = '{request.HttpRequestId}' ";
            }
            if (!string.IsNullOrWhiteSpace(request.HttpHost))
            {
                sqlModel += $" and HttpHost like '%{request.HttpHost}%' ";
            }
            if (!string.IsNullOrWhiteSpace(request.HttpPath))
            {
                sqlModel += $" and HttpPath like '%{request.HttpPath}%' ";
            }
            if (!string.IsNullOrWhiteSpace(request.Message))
            {
                sqlModel += $" and Message like '%{request.Message}%' ";
            }
            if (!string.IsNullOrWhiteSpace(request.SourceContext))
            {
                sqlModel += $" and SourceContext like '%{request.SourceContext}%' ";
            }
            if (request.Level.HasValue)
            {
                sqlModel += $" and Level = '{request.Level.Value}' ";
            }
            if (request.TimeStart.HasValue)
            {
                sqlModel += $" and `Timestamp` >= '{request.TimeStart}' ";
            }
            if (request.TimeEnd.HasValue)
            {
                sqlModel += $" and `Timestamp` <= '{request.TimeEnd}' ";
            }
            if (request.Limit <= 0)
            {
                request.Limit = 50;
            }
            if (request.Limit > 50)
            {
                request.Limit = 50;
            }

            string sqlStr = string.Empty;
            int idx = 0;
            foreach (var tableName in tableNames)
            {
                if (idx > 0)
                {
                    sqlStr += @$" union all ";
                }
                sqlStr += sqlModel.Replace("tableName_Placeholder", tableName);
                idx++;
            }
            sqlStr += $" order by `Timestamp` desc limit {request.Limit} ";

            var res = await QueryListAsync<LoggerSimpleDO>(sqlStr);
            return res;
        }
    }
}
