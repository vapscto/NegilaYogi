using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016
{
    public class CustomJsonConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            JObject result = new JObject();

            DateTime dd = (DateTime)value;

            result.Add("PASE_Date", JToken.FromObject(dd.ToString("MM/dd/yyyy")));
            //result.Add("DateTwo", JToken.FromObject(dd.DateTwo));
            //result.WriteTo(writer);
        }

        // Other JsonConverterMethods
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
           
            throw new NotImplementedException();
        }
    }
}

