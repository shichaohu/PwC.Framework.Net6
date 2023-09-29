using PwC.Crm.Share.PwcNetCore;
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
    public interface ICRMClientFactory
    {

        ICrequest GetCrequest(CRMClientTypeEnum clientType);
        ICRMClient GetCRMClient(CRMClientTypeEnum clientType);
        /// <summary>
        /// 获取DataBaseConnectString
        /// </summary>
        /// <param name="clientTypeEnum"></param>
        /// <returns></returns>
        string GetConnectString(CRMClientTypeEnum clientTypeEnum);
    }
}
