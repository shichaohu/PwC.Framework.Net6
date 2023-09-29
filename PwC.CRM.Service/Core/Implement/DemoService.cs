using CrmModels;
using PwC.CRM.Service.Dto.Request;

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
            var res1 = await _oDataHttpClient.QueryRecords<Systemuser>(fetchXml);
            var user = res1.Data[0];
            user.address1_name = "xx1";

            using (var tranSvcClient = TransactionServiceClient)
            {
                tranSvcClient.UpdateWithinTransaction(user);
                tranSvcClient.CommitTransaction();
            };

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