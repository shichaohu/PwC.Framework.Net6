using PwC.CRM.Share.CRMClients.OData;

namespace PwC.CRM.Share.CRMClients
{
    /// <summary>
    /// CRM客户端工厂
    /// </summary>
    public class CRMClientFactory : ICRMClientFactory
    {
        private Dictionary<CRMClientTypeEnum, IODataHttpClient> _oDataHttpClientDic;
        private Dictionary<CRMClientTypeEnum, string> _crmConnectionStringDic;
        public CRMClientFactory()
        {
            _oDataHttpClientDic = new Dictionary<CRMClientTypeEnum, IODataHttpClient> { };
            _crmConnectionStringDic = new Dictionary<CRMClientTypeEnum, string> { };

        }

        public void AddODataHttpClient(CRMClientTypeEnum clientType, IODataHttpClient client)
        {
            if (!_oDataHttpClientDic.ContainsKey(clientType))
            {
                _oDataHttpClientDic.Add(clientType, client);
            }
        }
        public IODataHttpClient GetODataHttpClient(CRMClientTypeEnum clientType)
        {
            if (_oDataHttpClientDic.ContainsKey(clientType))
            {
                return _oDataHttpClientDic[clientType];
            }
            else
            {
                return _oDataHttpClientDic[CRMClientTypeEnum.Default];
            }
        }
        public void AddCRMConnectionString(CRMClientTypeEnum clientType, string crmConnectionString)
        {
            if (!_crmConnectionStringDic.ContainsKey(clientType))
            {
                _crmConnectionStringDic.Add(clientType, crmConnectionString);
            }
        }

        /// <summary>
        /// 获取DataBaseConnectString
        /// </summary>
        /// <param name="clientTypeEnum"></param>
        /// <returns></returns>
        public string GetConnectString(CRMClientTypeEnum clientType = CRMClientTypeEnum.Default)
        {

            if (_crmConnectionStringDic.ContainsKey(clientType))
            {
                return _crmConnectionStringDic[clientType];
            }
            else
            {
                return _crmConnectionStringDic[CRMClientTypeEnum.Default];
            }
        }
    }
}
