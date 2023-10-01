using Newtonsoft.Json;

namespace PwC.CRM.Share.CRMClients.OData.Models
{
    internal class EntityReferenceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EntityReference);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            EntityReference entityReference = (EntityReference)value;
            writer.WriteValue(entityReference.PluralForm + "(" + entityReference.Id.ToString() + ")");
        }
    }
}
