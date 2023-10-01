using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.CRMClients.OData
{
    internal class CommonFun
    {
        public static string GetPluralForm(string entityName)
        {
            if (entityName.EndsWith("y"))
            {
                return entityName.Substring(0, entityName.Length - 1) + "ies";
            }

            if (entityName.EndsWith("s") || entityName.EndsWith("x") || entityName.EndsWith("sh"))
            {
                return entityName + "es";
            }

            return entityName + "s";
        }
    }
}
