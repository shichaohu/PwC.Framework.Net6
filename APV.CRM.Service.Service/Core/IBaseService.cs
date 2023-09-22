using APV.CRM.Service.Service.Enums;
using PwC.Crm.Share.CommonCode;
using PwcNetCore;

namespace APV.CRM.Service.Service.Core
{
    public interface IBaseService : IDependency
    {
        /// <summary>
        /// 是否存在实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<bool> AnyEntity<T>(List<(string conditionName, SQLConditionOperator conditionOperator, string conditionValue)> condition);
        /// <summary>
        /// 查询单个实体数据
        /// </summary>
        /// <typeparam name="T">标准的实体模型（CrmModels）</typeparam>
        /// <param name="fields">查询字段</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        Task<T> GetEntity<T>(List<string>? fields, List<(string conditionName, SQLConditionOperator conditionOperator, string conditionValue)> condition);
        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <typeparam name="T">标准的实体模型（CrmModels）</typeparam>
        /// <param name="fields">查询字段</param>
        /// <param name="condition">条件</param>
        /// <param name="top">前 top 条数据</param>
        /// <returns></returns>
        Task<List<T>> GetEntityList<T>(List<string>? fields, List<(string conditionName, SQLConditionOperator conditionOperator, string conditionValue)> condition, int? top = null);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<CrmResponse> UpdateEntity<T>(Guid id, T entity);
    }
}
