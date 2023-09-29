using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using PwC.Crm.Share.CRMClients.OData.Models;
using System.Reflection;
using System.ServiceModel;

namespace PwC.Crm.Share.CRMClients.Dataverse
{
    public class TransactionServiceClient : ServiceClient
    {
        private ExecuteTransactionRequest _executeTransactionRequest;

        /// <summary>
        /// ServiceClient  within a transaction to accept the connectionstring as a parameter
        /// </summary>
        /// <param name="dataverseConnectionString">dataverseConnectionString</param>
        /// <param name="logger">Logging provider Microsoft.Extensions.Logging.ILogger</param>
        public TransactionServiceClient(string dataverseConnectionString, ILogger logger = null)
            : base(dataverseConnectionString, logger)
        {
            InitServiceClient();
        }

        /// <summary>
        /// Init ServiceClient
        /// </summary>
        private void InitServiceClient()
        {
            //Create an empty organization request collection
            _executeTransactionRequest = new ExecuteTransactionRequest()
            {
                Requests = new OrganizationRequestCollection(),
                ReturnResponses = true,
                RequestName = ""
            };
        }
        /// <summary>
        /// Issues a Create request to Dataverse within a transaction
        /// </summary>
        /// <param name="entity">Entity to create</param>
        public void CreateWithinTransaction(Entity entity)
        {
            CreateRequest createRequest = new() { Target = entity, RequestName = entity.LogicalName };
            _executeTransactionRequest.Requests.Add(createRequest);
        }
        /// <summary>
        /// Issues a Create request to Dataverse within a transaction
        /// </summary>
        /// <param name="model">model to create</param>
        public void CreateWithinTransaction<T>(T model) where T : class, new()
        {
            var entity = ConvertToEntity(model);
            CreateWithinTransaction(entity);
        }

        /// <summary>
        /// Issues a Delete request to Dataverse within a transaction
        /// </summary>
        /// <param name="entityName">Entity name to delete</param>
        /// <param name="id">ID if entity to delete</param>
        public void DeleteWithinTransaction(string entityName, Guid id)
        {
            DeleteRequest createRequest = new() { RequestId = id, RequestName = entityName };
            _executeTransactionRequest.Requests.Add(createRequest);
        }

        /// <summary>
        /// Issues an update to Dataverse within a transaction
        /// </summary>
        /// <param name="entity">Entity to update into Dataverse</param>
        public void UpdateWithinTransaction(Entity entity)
        {
            UpdateRequest createRequest = new() { Target = entity, RequestName = entity.LogicalName };
            _executeTransactionRequest.Requests.Add(createRequest);
        }

        /// <summary>
        /// Issues an update to Dataverse within a transaction
        /// </summary>
        /// <param name="model">model to update into Dataverse</param>
        public void UpdateWithinTransaction<T>(T model) where T : class, new()
        {
            var entity = ConvertToEntity(model);
            UpdateWithinTransaction(entity);
        }

        public OrganizationResponseCollection CommitTransaction()
        {
            try
            {
                var responseForCreateRecords = (ExecuteTransactionResponse)base.Execute(_executeTransactionRequest);
                return responseForCreateRecords.Responses;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string errorMsg = $"Execute OrganizationService request failed for the index {((ExecuteTransactionFault)(ex.Detail)).FaultedRequestIndex + 1} and the reason being: {ex.Detail.Message}";
                throw new Exception(errorMsg);
            }
            finally
            {
                base.Dispose();
            }
        }
        public async Task<OrganizationResponseCollection> CommitTransactionAsync()
        {
            try
            {
                var responseForCreateRecords = (ExecuteTransactionResponse)(await base.ExecuteAsync(_executeTransactionRequest));
                return responseForCreateRecords.Responses;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string errorMsg = $"Execute OrganizationService request failed for the index {((ExecuteTransactionFault)(ex.Detail)).FaultedRequestIndex + 1} and the reason being: {ex.Detail.Message}";
                throw new Exception(errorMsg);
            }
            finally
            {
                base.Dispose();
            }
        }

        #region private
        private Entity ConvertToEntity<T>(T model) where T : class, new()
        {
            var entity = new Entity();
            List<PropertyInfo> list = (from w in (typeof(T)).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                       select w).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                string name = list[i].Name.ToLower();
                object value = list[i].GetValue(model);
                if (value == null)
                {
                    continue;
                }

                if (list[i].PropertyType.Name == "EntityReference")
                {
                    object[] customAttributes = list[i].GetCustomAttributes(typeof(CFieldType), inherit: true);
                    if (customAttributes.Length >= 0)
                    {
                        entity[name] = value.ToString();
                    }
                    continue;
                }

                if (list[i].PropertyType.IsEnum)
                {
                    entity[name] = new OptionSetValue((int)value);
                    continue;
                }

                entity[name] = value;
            }

            return entity;
        }
        #endregion
    }
}
