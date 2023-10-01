using Newtonsoft.Json;
using PwC.CRM.Share.Extensions;
using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Models.Table;

namespace PwC.CRM.Service.Core.Implement
{
    public class DemoService : BaseService, IDemoService
    {
        public DemoService(ICommonInjectionObject commonInjectionObject) : base(commonInjectionObject)
        {
        }
        public async Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter)
        {
            //时区转换 utc转当地时间
            string modifiedon = "8";
            string fetchXml = $@"
                <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                  <entity name='systemuser'>
                    <attribute name='systemuserid' />
                    <filter>
                      <condition attribute='employeeid' operator='eq' value='S1121072' />
                    </filter>
                  </entity>
                </fetch>";
            //ODataHttpClient
            var res1 = await _oDataHttpClient.QueryRecords<Systemuser>(fetchXml);
            var user = res1.Data[0];
            user.address1_name = "schtest00";
            var resUP = await _oDataHttpClient.UpdateRecord(user.systemuserid, user);


            Apv_Message message = new()
            {
                apv_name = $"schtest_可删除_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                apv_content = "",
                apv_messgtype = EnumMessgtype.PLM系统通知,
                ownerid = new CRM.Share.CRMClients.OData.Models.EntityReference(user.systemuserid),
                apv_uniquemarkcode = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            //事务批量创建、更新
            using (var tranSvcClient = TransactionServiceClient)
            {
                var userxx = tranSvcClient.Retrieve("systemuser", user.systemuserid.Value, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                var jsuser = userxx.ToModel<Systemuser>();

                tranSvcClient.CreateInTransaction(message);
                message.apv_content = "list";
                tranSvcClient.CreateInTransaction(new List<Apv_Message> { message });

                tranSvcClient.UpdateInTransaction(user);
                user.address1_name = "schtest02";
                tranSvcClient.UpdateInTransaction(new List<Systemuser> { user });

                tranSvcClient.DeleteInTransaction("apv_message", new Guid("6e83ccae-3560-ee11-8df0-000d3aa08d08"));
                var tranRresponse = tranSvcClient.CommitTransaction();
            };

            //CRequest
            var res2 = await _cRequest.QueryRecords<Systemuser>("systemuser", fetchXml);
            var user2 = res1.Data[0];
            Console.WriteLine(user2.address1_name);

            //查询自定义接口
            //object paramList = new object();
            //_cRequest.Execute<Systemuser>("api名称", paramList);


            return new List<Systemuser>();
        }


    }
}