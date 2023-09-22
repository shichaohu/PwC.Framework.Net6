using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.PwcNetCore.Models
{
    public enum ResultCode
    {
        Success = 1000,
        ParameterError = 2000,
        InternalError = 2010,
        OtherError = 2020,
        DataError = 2030
    }
}
