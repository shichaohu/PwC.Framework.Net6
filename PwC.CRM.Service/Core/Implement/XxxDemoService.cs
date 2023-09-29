﻿using PwC.CRM.Service.Dto.Request;
using PwC.Crm.Share.PwcNetCore;
using CrmModels;

namespace PwC.CRM.Service.Core.Implement
{
    public class XxxDemoService : BaseService, IXxxDemoService
    {
        public XxxDemoService(ICommonInjectionObject commonInjectionObject) : base(commonInjectionObject)
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
            var res1 = await _crmClient.QueryRecords<Systemuser>(fetchXml);
            var res11 = await _cRequest.QueryRecords<Systemuser>("systemuser", fetchXml);

            //查询自定义接口
            //object paramList = new object();
            //_cRequest.Execute<Systemuser>("api名称", paramList);


            return new List<Systemuser>();
        }


    }
}