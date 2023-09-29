using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Service.Core.DapperRepositorys
{
    public class DapperRepository : IDapperRepository
    {
        public IConfiguration Configuration { get; }
        public string LogTableName { get; }
        private string _connectionString { get; }
        public DapperRepository(IConfiguration Configuration)
        {
            Configuration = Configuration;
            /// 数据库连接字符串
            _connectionString = Configuration["Log:MySql:DbConnectionString"];
            LogTableName = Configuration["Log:MySql:TableName"];
        }

        private IDbConnection _connection { get; set; }
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    _connection = new MySqlConnection(_connectionString);
                }
                return _connection;
            }
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="beginTransaction"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<T> QueryList<T>(string sql, object param, CommandType? commandType = null, bool beginTransaction = false) where T : class
        {
            try
            {
                return Execute((conn, dbTransaction) =>
                {
                    return conn.Query<T>(sql, param, dbTransaction, commandType: commandType ?? CommandType.Text).ToList();
                }, beginTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 异步获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<T>> QueryListAsync<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var result = await conn.QueryAsync<T>(sql, param, transaction, commandType: commandType ?? CommandType.Text);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private T Execute<T>(Func<IDbConnection, IDbTransaction, T> func, bool beginTransaction = false, bool disposeConn = true)
        {
            IDbTransaction dbTransaction = null;
            if (beginTransaction)
            {
                Connection.Open();
                dbTransaction = Connection.BeginTransaction();
            }
            try
            {
                T reslutT = func(Connection, dbTransaction);
                dbTransaction?.Commit();
                return reslutT;
            }
            catch (Exception ex)
            {
                dbTransaction?.Rollback();
                Connection.Dispose();
                throw ex;
            }
            finally
            {
                if (disposeConn)
                {
                    Connection.Dispose();
                }
            }
        }
    }
}
