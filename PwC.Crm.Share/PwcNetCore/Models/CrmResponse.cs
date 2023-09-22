using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.PwcNetCore.Models
{
    public class CrmResponse : BassInfo
    {
        public Guid? Id { get; set; }
    }
    public class CrmResponse<T> : CrmResponse
    {
        public List<T> value { get; set; }
    }
}
