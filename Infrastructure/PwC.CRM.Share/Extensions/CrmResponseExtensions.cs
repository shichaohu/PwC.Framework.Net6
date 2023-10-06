using PwC.CRM.Share.CRMClients.OData.Models;

namespace PwC.CRM.Share.Extensions;

public static class CrmResponseExtensions
{

    public static bool HasError<T>(this CrmResponse<T> res) =>
        res == null
        || res.Code != ResultCode.Success
        || res.Data == null
        || res.Data.Count == 0;


    public static bool HasError(this CrmResponse res) =>
    res == null
    || res.Code != ResultCode.Success;

}

