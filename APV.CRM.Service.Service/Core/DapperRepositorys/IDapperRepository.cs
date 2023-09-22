using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APV.CRM.Service.Service.Core.DapperRepositorys
{
    public interface IDapperRepository
    {
        /// 获取数据列表
        List<T> QueryList<T>(string sql, object param, CommandType? commandType = null, bool beginTransaction = false) where T : class;
        ///异步获取数据列表
        Task<List<T>> QueryListAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null) where T : class;
    }

}
