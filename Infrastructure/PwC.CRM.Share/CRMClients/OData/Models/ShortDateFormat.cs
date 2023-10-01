using Newtonsoft.Json.Converters;

namespace PwC.CRM.Share.CRMClients.OData.Models
{
    public class ShortDateFormat : IsoDateTimeConverter
    {
        public ShortDateFormat()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }

        public ShortDateFormat(string format)
        {
            base.DateTimeFormat = format;
        }
    }
}
