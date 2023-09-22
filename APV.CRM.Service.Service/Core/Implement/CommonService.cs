using CrmModels;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using PwcNetCore;

namespace APV.CRM.Service.Service.Core.Implement
{
    public sealed class CommonService : BaseService, ICommonService
    {
        public CommonService(ICommonInjectionObject commonInjectionObject)
            : base(commonInjectionObject)
        {

        }
        #region user

        /// <summary>
        /// 查询crm的用户信息(只包含用户id)
        /// </summary>
        /// <param name="userNo">用户工号,如 S1122959</param>
        /// <returns></returns>
        public async Task<Systemuser> QueryUserOnlyId(string userNo)
        {
            string fetchXml = $@"
                <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                  <entity name='systemuser'>
                    <attribute name='systemuserid' />
                    <filter>
                      <condition attribute='employeeid' operator='eq' value='{userNo}' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await _cRequest.QueryRecords<Systemuser>("systemuser", fetchXml);
            if (result.code != ResultCode.Success || result.value.Count == 0)
            {
                return null;
            }

            return result.value.First();
        }
        /// <summary>
        /// 查询crm的用户信息(全部字段)
        /// </summary>
        /// <param name="userNo">用户工号,如 S1122959</param>
        /// <returns></returns>
        public async Task<Systemuser> QueryUser(string userNo)
        {
            string fetchXml = $@"
                <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                  <entity name='systemuser'>
                    <all-attributes />
                    <filter>
                      <condition attribute='employeeid' operator='eq' value='{userNo}' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await _cRequest.QueryRecords<Systemuser>("systemuser", fetchXml);
            if (result.code != ResultCode.Success || result.value.Count == 0)
            {
                string message = $"未在CRM中查询到工号({userNo})对应的用户信息";
                throw new Exception(message);
            }

            return result.value.First();
        }
        #endregion

        /// <summary>
        /// 查询crm的币种信息(只包含币种id)
        /// </summary>
        /// <param name="isoCurrencyCode">币种代码，如：CNY</param>
        /// <returns></returns>
        public async Task<Transactioncurrency> QueryCurrencyOnlyId(string isoCurrencyCode)
        {
            string fetchXml = $@"
                <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                  <entity name='transactioncurrency'>
                    <attribute name='transactioncurrencyid' />
                    <filter>
                      <condition attribute='isocurrencycode' operator='eq' value='{isoCurrencyCode}' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await _cRequest.QueryRecords<Transactioncurrency>("transactioncurrency", fetchXml);
            if (result.code != ResultCode.Success || result.value.Count == 0)
            {
                string message = $"未在CRM中查询到货币代码({isoCurrencyCode})对应的货币信息";
                throw new Exception(message);
            }

            return result.value.First();
        }
        /// <summary>
        /// 查询entity的文件附件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<List<Fileattachment>> QueryFileAttachmentsOfEntity(Guid entityId, string entityName)
        {
            string relationshipName = $"{entityName}_FileAttachments";
            var relationshipQueryCollection = new Microsoft.Xrm.Sdk.RelationshipQueryCollection {
                {
                    new Microsoft.Xrm.Sdk.Relationship(relationshipName),
                    new QueryExpression("fileattachment"){
                        ColumnSet = new ColumnSet(
                                        "createdon",
                                        "mimetype",
                                        "filesizeinbytes",
                                        "filename",
                                        "regardingfieldname",
                                        "fileattachmentid",
                                        "body")
                    }
                }
            };
            RetrieveRequest request = new RetrieveRequest
            {
                ColumnSet = new ColumnSet($"{entityName}id"),
                RelatedEntitiesQuery = relationshipQueryCollection,
                Target = new Microsoft.Xrm.Sdk.EntityReference(entityName, entityId)
            };

            // Send the request
            RetrieveResponse response = (RetrieveResponse)(await ServiceClient.ExecuteAsync(request));

            //Display related FileAttachment data for the record
            var result = response.Entity.RelatedEntities[new Microsoft.Xrm.Sdk.Relationship(relationshipName)]
                .Entities.Select(e => new Fileattachment
                {
                    createdon = e.GetAttributeValue<DateTime>("createdon"),
                    mimetype = e.GetAttributeValue<string>("mimetype"),
                    filesizeinbytes = e.GetAttributeValue<long>("filesizeinbytes"),
                    filename = e.GetAttributeValue<string>("filename"),
                    regardingfieldname = e.GetAttributeValue<string>("regardingfieldname"),
                    fileattachmentid = e.GetAttributeValue<Guid>("fileattachmentid"),
                    body = e.GetAttributeValue<string>("body")
                }).ToList();

            return result;
        }

        /// <summary>
        /// Downloads a file or image
        /// </summary>
        /// <param name="entityId">entity id</param>
        /// <param name="entityName">entity logical name</param>
        /// <param name="attributeName">The name of the file or image column</param>
        /// <returns></returns>
        public byte[] DownloadFile(Guid entityId, string entityName, string attributeName)
        {
            using var service = ServiceClient;
            InitializeFileBlocksDownloadRequest initializeFileBlocksDownloadRequest = new()
            {
                Target = new Microsoft.Xrm.Sdk.EntityReference(entityName, entityId),
                FileAttributeName = attributeName
            };

            var initializeFileBlocksDownloadResponse = (InitializeFileBlocksDownloadResponse)service.Execute(initializeFileBlocksDownloadRequest);

            string fileContinuationToken = initializeFileBlocksDownloadResponse.FileContinuationToken;
            long fileSizeInBytes = initializeFileBlocksDownloadResponse.FileSizeInBytes;

            List<byte> fileBytes = new((int)fileSizeInBytes);

            long offset = 0;
            // If chunking is not supported, chunk size will be full size of the file.
            long blockSizeDownload = !initializeFileBlocksDownloadResponse.IsChunkingSupported ? fileSizeInBytes : 4 * 1024 * 1024;

            // File size may be smaller than defined block size
            if (fileSizeInBytes < blockSizeDownload)
            {
                blockSizeDownload = fileSizeInBytes;
            }

            while (fileSizeInBytes > 0)
            {
                // Prepare the request
                DownloadBlockRequest downLoadBlockRequest = new()
                {
                    BlockLength = blockSizeDownload,
                    FileContinuationToken = fileContinuationToken,
                    Offset = offset
                };

                // Send the request
                var downloadBlockResponse =
                         (DownloadBlockResponse)service.Execute(downLoadBlockRequest);

                // Add the block returned to the list
                fileBytes.AddRange(downloadBlockResponse.Data);

                // Subtract the amount downloaded,
                // which may make fileSizeInBytes < 0 and indicate
                // no further blocks to download
                fileSizeInBytes -= (int)blockSizeDownload;
                // Increment the offset to start at the beginning of the next block.
                offset += blockSizeDownload;
            }

            return fileBytes.ToArray();
        }

    }
}
