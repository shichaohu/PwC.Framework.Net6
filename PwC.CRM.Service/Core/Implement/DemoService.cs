using Newtonsoft.Json;
using PwC.CRM.Share.Extensions;
using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Models.Table;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace PwC.CRM.Service.Core.Implement
{
    public class DemoService : BaseService, IDemoService
    {
        public DemoService(ICommonInjectionObject commonInjectionObject) : base(commonInjectionObject)
        {
        }
        public async Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter)
        {
            string fetchXml = $@"
                <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                  <entity name='systemuser'>
                    <all-attributes />
                    <filter>
                      <condition attribute='employeeid' operator='eq' value='S1121072' />
                    </filter>
                  </entity>
                </fetch>";
            //ODataHttpClient fetchXml查询
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

            //事务批量创建、更新，需将提交逻辑放在using语句块里面
            using (var tranSvcClient = TransactionServiceClient)
            {
                //fetchXml inner join查询
                string fetchXml2 = $@"
                                    <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                                      <entity name='apv_message'>
                                        <attribute name='apv_messageid' />
                                        <link-entity name='systemuser' to='ownerid' from='systemuserid' alias='link_owner' link-type='inner'>
                                          <all-attributes />
                                          <filter>
                                            <condition attribute='systemuserid' operator='eq' value='573e9425-9deb-ed11-8849-6045bd20a09e' />
                                          </filter>
                                        </link-entity>
                                      </entity>
                                    </fetch>
                                    ";
                FetchExpression query = new(fetchXml2);
                EntityCollection results = tranSvcClient.RetrieveMultiple(query);
                var userList = results.Entities.ToModelList<Link_Apv_Message>();
                var link_owner = userList[0].link_owner;

                //查询单条记录
                var userxx = tranSvcClient.Retrieve("systemuser", user.systemuserid.Value, new ColumnSet(true));
                var jsuser = userxx.ToModel<Systemuser>();

                //事务内创建
                tranSvcClient.CreateInTransaction(message);
                message.apv_content = "list";
                tranSvcClient.CreateInTransaction(new List<Apv_Message> { message });

                //事务内更新
                tranSvcClient.UpdateInTransaction(user);
                user.address1_name = "schtest02";
                tranSvcClient.UpdateInTransaction(new List<Systemuser> { user });

                //事务内删除
                tranSvcClient.DeleteInTransaction("apv_message", new Guid("6e83ccae-3560-ee11-8df0-000d3aa08d08"));

                //提交事务，不写时，会自动提交
                //如果需要读取并使用事务提交结果，则使用此行代码显示提交
                //如不关心提交结果，则忽略此行代码
                //var tranResponse = tranSvcClient.CommitTransaction();
            };

            //查询自定义接口
            //object paramList = new object();
            //_oDataHttpClient.Execute<Systemuser>("api名称", paramList);


            return new List<Systemuser>();
        }


    }
}