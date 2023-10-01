using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.CRMClients.OData.Models
{
    public enum ResultCode
    {
        Success = 200,
        ParameterError = 4000,
        InternalError = 4010,
        OtherError = 4020,
        DataError = 4030
    }
}
