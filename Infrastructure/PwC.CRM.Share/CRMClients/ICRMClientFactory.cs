using PwC.CRM.Share.CRMClients.OData;

namespace PwC.CRM.Share.CRMClients
{
    /// <summary>
    /// CRM客户端工厂
    /// </summary>
    public interface ICRMClientFactory
    {
        IODataHttpClient GetODataHttpClient(CRMClientTypeEnum clientType);
        /// <summary>
        /// 获取DataBaseConnectString
        /// </summary>
        /// <param name="clientTypeEnum"></param>
        /// <returns></returns>
        string GetConnectString(CRMClientTypeEnum clientTypeEnum);
    }
}
