using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using PwC.Crm.Share.CRMClients;
using PwC.Crm.Share.CRMClients.Dataverse;
using PwC.Crm.Share.CRMClients.OData;
using PwC.CRM.Service.Enums;
using PwcNetCore;

namespace PwC.CRM.Service.Core.Implement
{
    public class BaseService : IBaseService
    {
        protected readonly IConfiguration _configuration;
        protected readonly ICrequest _cRequest;
        protected readonly IODataHttpClient _oDataHttpClient;
        protected readonly ICRMClientFactory _crmClientFactory;
        protected readonly string _crmConnectionString;
        protected ServiceClient ServiceClient
        {
            get
            {
                return new ServiceClient(_crmConnectionString);
            }
        }

        protected TransactionServiceClient TransactionServiceClient
        {
            get
            {
                return new TransactionServiceClient(_crmConnectionString);
            }
        }

        public BaseService(ICommonInjectionObject commonInjectionObject)
        {
            _configuration = commonInjectionObject.Configuration;
            _crmClientFactory = commonInjectionObject.CrmClientFactory;

            if (commonInjectionObject?.HttpContextAccessor?.HttpContext?.Request != null)
            {
                string targetCRMService = commonInjectionObject.HttpContextAccessor.HttpContext.Request.Headers["Target-CRM-Service"];
                if (Enum.TryParse(targetCRMService, out CRMClientTypeEnum crmClientTypeEnum))
                {
                    _crmConnectionString = _crmClientFactory.GetConnectString(crmClientTypeEnum);
                    _cRequest = _crmClientFactory.GetCrequest(crmClientTypeEnum);
                    _oDataHttpClient = _crmClientFactory.GetODataHttpClient(crmClientTypeEnum);
                }
            }

            _crmConnectionString ??= _crmClientFactory.GetConnectString(CRMClientTypeEnum.Default);
            _cRequest ??= _crmClientFactory.GetCrequest(CRMClientTypeEnum.Default);
            _oDataHttpClient ??= _crmClientFactory.GetODataHttpClient(CRMClientTypeEnum.Default);


        }
        /// <summary>
        /// 是否存在实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<bool> AnyEntity<T>(List<(string conditionName, SQLConditionOperator conditionOperator, string conditionValue)> condition)
        {
            int? top = 1;
            List<string> fields = new()
            {
                $"{typeof(T).Name.ToLower()}id"
            };
            var result = await GetEntityList<T>(fields, condition, top);
            return result?.Count > 0;
        }

        public async Task<T> GetEntity<T>(List<string>? fields, List<(string conditionName, SQLConditionOperator conditionOperator, string conditionValue)> condition)
        {
            int? top = 1;
            var result = await GetEntityList<T>(fields, condition, top);
            return result == null ? default : result.FirstOrDefault();
        }
        public async Task<List<T>> GetEntityList<T>(List<string>? fields, List<(string conditionName, SQLConditionOperator conditionOperator, string conditionValue)> condition, int? top = null)
        {
            var entityName = typeof(T).Name.ToLower();

            string topStr = string.Empty;
            if (top.HasValue)
            {
                topStr = $" top='{top}' ";
            }

            string queryFields = $"<all-attributes />";
            if (fields?.Count > 0)
            {
                queryFields = string.Join("", fields.Select(x => $" <attribute name='{x.ToLower()}' /> "));
            }

            string filterConditions = string.Empty;
            if (condition?.Count > 0)
            {
                string conditionAttributes = string.Join("",
                    condition.Select(x =>
                                        {
                                            if (x.conditionOperator == SQLConditionOperator.In)
                                            {
                                                var inValues = x.conditionValue.Split(",");
                                                if (inValues?.Length > 0)
                                                {
                                                    string inValueConditions = string.Join("", inValues.Select(y => $"<value>{y}</value>"));
                                                    return @$"<condition attribute='{x.conditionName}' operator='in'>
                                                                {inValueConditions}
                                                              </condition>";
                                                }
                                                else
                                                    return "";
                                            }
                                            else
                                            {
                                                return $" <condition attribute='{x.conditionName}' operator='{x.conditionOperator.ToString().ToLower()}' value='{x.conditionValue}' /> ";
                                            }
                                        })
                    );
                filterConditions = $@"
                    <filter>
                      {conditionAttributes}
                    </filter>";
            }


            string fetchXml = $@"
                <fetch xmlns:generator='MarkMpn.SQL4CDS' {topStr} >
                  <entity name='{entityName}'>
                    {queryFields}
                    {filterConditions}
                  </entity>
                </fetch>";
            var result = await _cRequest.QueryRecords<T>(entityName, fetchXml);

            if (result.code != ResultCode.Success || result.value == null)
            {
                string message = $"查询实体表({entityName})出错：{result.message}";
                throw new Exception(message);
            }
            return result.value;

        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<CrmResponse> UpdateEntity<T>(Guid id, T entity)
        {
            var result = await _cRequest.UpdateRecords(id, entity);
            return result;
        }

    }
}
