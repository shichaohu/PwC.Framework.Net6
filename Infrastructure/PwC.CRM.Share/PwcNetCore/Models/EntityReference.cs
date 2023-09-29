using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.PwcNetCore.Models
{
    public class EntityReference
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string logicalName { get; set; }

        public string PluralForm { get; set; }

        public EntityReference()
        {
        }

        public EntityReference(Guid? id)
        {
            if (id.HasValue)
            {
                Id = id;
            }
        }
    }
}
