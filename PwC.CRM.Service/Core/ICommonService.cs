using PwC.CRM.Share.CommonCode;
using PwC.CRM.Models.Table;

namespace PwC.CRM.Service.Core
{
    public interface ICommonService : IDependency
    {
        
        /// <summary>
        /// 查询crm的用户信息(只包含用户id)
        /// </summary>
        /// <param name="userNo">用户工号,如 S1122959</param>
        /// <returns></returns>
        Task<Systemuser> QueryUserOnlyId(string userNo);
        /// <summary>
        /// 查询crm的用户信息(全部字段)
        /// </summary>
        /// <param name="userNo">用户工号,如 S1122959</param>
        /// <returns></returns>
        Task<Systemuser> QueryUser(string userNo);

        /// <summary>
        /// 查询crm的币种信息(只包含币种id)
        /// </summary>
        /// <param name="isoCurrencyCode">币种代码，如：CNY</param>
        /// <returns></returns>
        Task<Transactioncurrency> QueryCurrencyOnlyId(string isoCurrencyCode);
        /// <summary>
        /// 查询entity的文件附件
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        Task<List<Fileattachment>> QueryFileAttachmentsOfEntity(Guid entityId, string entityName);

        /// <summary>
        /// Downloads a file or image
        /// </summary>
        /// <param name="entityId">entity id</param>
        /// <param name="entityName">entity logical name</param>
        /// <param name="attributeName">The name of the file or image column</param>
        /// <returns></returns>
        byte[] DownloadFile(Guid entityId, string entityName, string attributeName);

    }
}
