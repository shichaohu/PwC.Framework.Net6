using PwcNetCore;

namespace PwC.CRM.Share.Extensions;

public static class CrmResponseExtensions
{

    public static bool HasError<T>(this CrmResponse<T> res) =>
        res == null
        || res.code != ResultCode.Success
        || res.value == null
        || res.value.Count == 0;


    public static bool HasError(this CrmResponse res) =>
    res == null
    || res.code != ResultCode.Success;

}

