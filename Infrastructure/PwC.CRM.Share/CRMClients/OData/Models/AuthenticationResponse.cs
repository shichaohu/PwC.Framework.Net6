using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.CRMClients.OData.Models
{
    internal class AuthenticationResponse
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public int expires_on { get; set; }

        public int ext_expires_in { get; set; }

        public int not_before { get; set; }

        public string resource { get; set; }

        public string token_type { get; set; }
    }
}
