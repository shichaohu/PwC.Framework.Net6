﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.CRMClients.OData.Models
{
    public class CFieldType : Attribute
    {
        public string FieldType { get; set; }

        public string EntityName { get; set; }

        public Type EnumType { get; set; }
    }
}
