using PwC.Crm.Share.PwcNetCore;
using PwcNetCore;

namespace PwC.Crm.Share.CRMClients
{
    /// <summary>
    /// CRM客户端工厂
    /// </summary>
    public class CRMClientFactory : ICRMClientFactory
    {
        private Dictionary<CRMClientTypeEnum, ICrequest> _crequestDic;
        private Dictionary<CRMClientTypeEnum, ICRMClient> _crmClientDic;
        private Dictionary<CRMClientTypeEnum, string> _crmConnectionStringDic;
        public CRMClientFactory()
        {
            _crequestDic = new Dictionary<CRMClientTypeEnum, ICrequest> { };
            _crmClientDic = new Dictionary<CRMClientTypeEnum, ICRMClient> { };
            _crmConnectionStringDic = new Dictionary<CRMClientTypeEnum, string> { };

        }

        public void AddCrequest(CRMClientTypeEnum clientType, ICrequest client)
        {
            if (!_crequestDic.ContainsKey(clientType))
            {
                _crequestDic.Add(clientType, client);
            }
        }
        public ICrequest GetCrequest(CRMClientTypeEnum clientType)
        {
            if (_crequestDic.ContainsKey(clientType))
            {
                return _crequestDic[clientType];
            }
            else
            {
                return _crequestDic[CRMClientTypeEnum.Default];
            }
        }
        public void AddCRMClient(CRMClientTypeEnum clientType, ICRMClient client)
        {
            if (!_crmClientDic.ContainsKey(clientType))
            {
                _crmClientDic.Add(clientType, client);
            }
        }
        public ICRMClient GetCRMClient(CRMClientTypeEnum clientType)
        {
            if (_crmClientDic.ContainsKey(clientType))
            {
                return _crmClientDic[clientType];
            }
            else
            {
                return _crmClientDic[CRMClientTypeEnum.Default];
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
