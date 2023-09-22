using Microsoft.Xrm.Sdk.Discovery;
using PwcNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.CRMClients
{
    /// <summary>
    /// CRM客户端工厂
    /// </summary>
    public class CRMClientFactory : ICRMClientFactory
    {
        private Dictionary<CRMClientTypeEnum, ICrequest> clientDic;
        private Dictionary<CRMClientTypeEnum, string> crmConnectionStringDic;
        public CRMClientFactory()
        {
            clientDic = new Dictionary<CRMClientTypeEnum, ICrequest> { };
            crmConnectionStringDic = new Dictionary<CRMClientTypeEnum, string> { };

        }

        public void AddCRMClient(CRMClientTypeEnum clientType, ICrequest client)
        {
            if (!clientDic.ContainsKey(clientType))
            {
                clientDic.Add(clientType, client);
            }
        }
        public ICrequest GetCRMClient(CRMClientTypeEnum clientType)
        {
            if (clientDic.ContainsKey(clientType))
            {
                return clientDic[clientType];
            }
            else
            {
                return clientDic[CRMClientTypeEnum.HK];
            }
        }


        public void AddCRMConnectionString(CRMClientTypeEnum clientType, string crmConnectionString)
        {
            if (!crmConnectionStringDic.ContainsKey(clientType))
            {
                crmConnectionStringDic.Add(clientType, crmConnectionString);
            }
        }

        /// <summary>
        /// 获取DataBaseConnectString
        /// </summary>
        /// <param name="clientTypeEnum"></param>
        /// <returns></returns>
        public string GetConnectString(CRMClientTypeEnum clientType = CRMClientTypeEnum.Default)
        {

            if (crmConnectionStringDic.ContainsKey(clientType))
            {
                return crmConnectionStringDic[clientType];
            }
            else
            {
                return crmConnectionStringDic[CRMClientTypeEnum.Default];
            }
        }
    }
}
