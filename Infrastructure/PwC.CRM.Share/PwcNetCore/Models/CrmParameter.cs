using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.PwcNetCore.Models
{
    public class CrmParameter
    {
        public bool BypassCustomPluginExecution { get; set; }

        public string MSCRMCallerID { get; set; }

        public bool include { get; set; }
    }
}
