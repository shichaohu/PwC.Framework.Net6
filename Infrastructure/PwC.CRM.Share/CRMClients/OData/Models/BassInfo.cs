using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.CRMClients.OData.Models
{
    public class BassInfo
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Location { get; set; }

        public ResultCode Code { get; set; }
    }
}
