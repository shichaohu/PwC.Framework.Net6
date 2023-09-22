using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.PwcNetCore.Models
{
    public class BassInfo
    {
        public string message { get; set; }
        public string stacktrace { get; set; }
        public string location { get; set; }

        public ResultCode code { get; set; }
    }
}
