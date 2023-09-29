using PwC.Crm.Share.PwcNetCore.Models;

namespace PwC.Crm.Share.PwcNetCore
{
    /// <summary>
    /// CRM Client
    /// </summary>
    public interface ICRMClient : IDisposable
    {
        Task<CrmResponse> CreateRecord<T>(string entityName, T entity, CrmParameter crmParameter = null);

        Task<CrmResponse> CreateRecord<T>(T entity, CrmParameter crmParameter = null);

        Task<CrmResponse<T>> QueryRecords<T>(string fetchXml, bool include);

        Task<CrmResponse<T>> QueryRecords<T>(string entityName, string fetchXml, bool include);

        Task<CrmResponse<T>> QueryRecords<T>(string fetchXml, CrmParameter crmParameter = null);

        Task<CrmResponse<T>> QueryRecords<T>(string entityName, string fetchXml, CrmParameter crmParameter = null);

        Task<CrmResponse<T>> QueryRecords<T>(string entityName, Guid? Id, string fields, CrmParameter crmParameter = null);

        Task<CrmResponse<T>> QueryRecords<T>(string entityName, Guid? Id, string[] fieldList, CrmParameter crmParameter = null);

        Task<CrmResponse<T>> QueryRecords<T>(Guid? Id, string[] fieldList, CrmParameter crmParameter = null);

        Task<CrmResponse<T>> QueryRecords<T>(Guid? Id, string fields, CrmParameter crmParameter = null);

        Task<CrmResponse> UpdateRecord<T>(string entityName, Guid? Id, T entity);

        Task<CrmResponse> UpdateRecord<T>(Guid? Id, T entity);

        Task<CrmResponse> UpdateRecord<T>(string entityName, Guid? Id, T entity, CrmParameter crmParameter);

        Task<CrmResponse> UpdateRecord<T>(Guid? Id, T entity, CrmParameter crmParameter);

        Task<CrmResponse> DeleteRecords(string entityName, Guid? Id);

        Task<CrmResponse> DeleteRecords(string entityName, Guid? Id, CrmParameter crmParameter);

        Task<CrmResponse> ClearField<T>(string entityName, Guid? Id, List<string> field);

        Task<CrmResponse<T>> Execute<T>(string operationName, object boundParameter, CrmParameter crmParameter = null);

        Task<CrmResponse> Associate(string entityName, Guid entityId, string relationship, EntityReference entityReferences, CrmParameter crmParameter = null);

        Task<CrmResponse> Disassociate(string entityName, Guid entityId, string relationship, EntityReference entityReferences, CrmParameter crmParameter = null);

        Task<CrmResponse<string>> ExecuteBatch(List<BatchContainer> data, CrmParameter crmParameter = null);
    }
}
