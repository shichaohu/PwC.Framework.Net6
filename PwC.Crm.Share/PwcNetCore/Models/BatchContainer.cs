using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.PwcNetCore.Models
{
    public class BatchContainer
    {
        public PerformOperations Operate { get; set; }

        public object data { get; set; }

        public Guid? Id { get; set; }

        public string? entityName { get; set; }
    }
}
