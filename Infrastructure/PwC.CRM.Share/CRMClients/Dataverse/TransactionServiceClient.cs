using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using PwC.CRM.Share.CRMClients.OData.Models;
using PwC.CRM.Share.Util;
using System.Reflection;
using System.ServiceModel;
using EntityReference = Microsoft.Xrm.Sdk.EntityReference;

namespace PwC.CRM.Share.CRMClients.Dataverse
{
    public class TransactionServiceClient : ServiceClient, IDisposable
    {
        private ExecuteTransactionRequest _executeTransactionRequest;
        private bool _hasTransactionCommit;

        /// <summary>
        /// ServiceClient  in a transaction to accept the connectionstring as a parameter
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
                ReturnResponses = true
            };
        }
        #region Create
        /// <summary>
        /// Create one entity into Dataverse in a transaction
        /// </summary>
        /// <param name="entity">entity to create</param>
        public void CreateInTransaction(Entity entity)
        {
            var entityCollection = new EntityCollection()
            {
                EntityName = entity.LogicalName,
                Entities = { entity }
            };
            CreateRequest request = new() { Target = entityCollection[0] };
            _executeTransactionRequest.Requests.Add(request);
        }
        /// <summary>
        /// Create multiple entities into Dataverse in a transaction
        /// </summary>
        /// <param name="entities">entities to create</param>
        public void CreateInTransaction(List<Entity> entities)
        {
            if (entities?.Count > 0)
            {
                var entityCollection = new EntityCollection()
                {
                    EntityName = entities[0].LogicalName
                };
                entityCollection.Entities.AddRange(entities);
                foreach (var entity in entityCollection.Entities)
                {
                    CreateRequest request = new() { Target = entity };
                    _executeTransactionRequest.Requests.Add(request);
                }
            }
        }
        /// <summary>
        /// Create one model into Dataverse in a transaction
        /// </summary>
        /// <param name="model">model to create</param>
        public void CreateInTransaction<T>(T model) where T : class, new()
        {
            var entity = ConvertToEntity(model);
            CreateInTransaction(entity);
        }
        /// <summary>
        /// Create multiple models into Dataverse in a transaction
        /// </summary>
        /// <param name="model">model to create</param>
        public void CreateInTransaction<T>(List<T> models) where T : class, new()
        {
            var entities = models.Select(x => ConvertToEntity(x)).ToList();
            CreateInTransaction(entities);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete one from Dataverse in a transaction
        /// </summary>
        /// <param name="entityName">name of entity name to delete</param>
        /// <param name="id">id of entity to delete</param>
        public void DeleteInTransaction(string entityName, Guid id)
        {
            DeleteRequest request = new() { Target = new EntityReference(entityName, id) };
            _executeTransactionRequest.Requests.Add(request);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update one entity into Dataverse in a transaction
        /// </summary>
        /// <param name="entity">entity to update</param>
        public void UpdateInTransaction(Entity entity)
        {
            var entityCollection = new EntityCollection()
            {
                EntityName = entity.LogicalName,
                Entities = { entity }
            };
            UpdateRequest request = new() { Target = entityCollection[0] };
            _executeTransactionRequest.Requests.Add(request);
        }
        /// <summary>
        /// Update multiple entities into Dataverse in a transaction
        /// </summary>
        /// <param name="entities">entities to update</param>
        public void UpdateInTransaction(List<Entity> entities)
        {

            if (entities?.Count > 0)
            {
                var entityCollection = new EntityCollection()
                {
                    EntityName = entities[0].LogicalName
                };
                entityCollection.Entities.AddRange(entities);
                foreach (var entity in entityCollection.Entities)
                {
                    UpdateRequest request = new() { Target = entity };
                    _executeTransactionRequest.Requests.Add(request);
                }
            }
        }

        /// <summary>
        /// Update one into Dataverse in a transaction
        /// </summary>
        /// <param name="model">model to update</param>
        public void UpdateInTransaction<T>(T model) where T : class, new()
        {
            var entity = ConvertToEntity(model);
            UpdateInTransaction(entity);
        }
        /// <summary>
        /// Update multiple models into Dataverse in a transaction
        /// </summary>
        /// <param name="model">model to create</param>
        public void UpdateInTransaction<T>(List<T> models) where T : class, new()
        {
            var entities = models.Select(x => ConvertToEntity(x)).ToList();
            UpdateInTransaction(entities);
        }
        #endregion


        public OrganizationResponseCollection CommitTransaction()
        {
            if (_hasTransactionCommit) return default;
            _hasTransactionCommit = true;
            try
            {
                var responseForCreateRecords = (ExecuteTransactionResponse)base.Execute(_executeTransactionRequest);
                return responseForCreateRecords.Responses;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string errorMsg = $"Execute TransactionServiceClient.CommitTransaction failed for the index {((ExecuteTransactionFault)(ex.Detail)).FaultedRequestIndex + 1} and the reason being: {ex.Detail.Message}";
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
            entity.LogicalName = GetEntityName<T>();
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

                var fieldTypes = list[i].GetCustomAttributes();
                CFieldType fieldType = null;
                foreach (var attr in fieldTypes)
                {
                    if (attr.GetType().Name == "CFieldType")
                    {
                        try
                        {
                            var ft = attr.CopyAs<CFieldType>();
                            if (ft != null)
                                fieldType = (CFieldType)attr;
                        }
                        catch { }
                    }
                }
                if (list[i].PropertyType.Name == "EntityReference")
                {
                    if (fieldType != null && Guid.TryParse(value.ToString(), out Guid erId))
                    {
                        entity[name] = new EntityReference(fieldType.EntityName, erId);
                    }
                    continue;
                }

                if (fieldType?.EnumType != null)
                {
                    if (Enum.TryParse(fieldType.EnumType, value.ToString(), true, out object valueEnum))
                        entity[name] = new OptionSetValue(valueEnum.GetHashCode());
                    continue;
                }

                entity[name] = value;
            }

            return entity;
        }

        private string GetEntityName<T>()
        {
            Type type = typeof(T);
            MethodInfo method = type.GetMethod("GetEntityKey");
            string entityName;
            if (method != null)
            {
                object obj = method.Invoke(null, null);
                entityName = obj.ToString();
            }
            else
            {
                entityName = type.Name.ToLower();
            }

            return entityName;
        }

        #endregion
        public void Dispose()
        {
            if (!_hasTransactionCommit)
            {
                CommitTransaction();
            }
        }
    }
}
