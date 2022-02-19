using System;
using System.Drawing;
using Newtonsoft.Json;

namespace CommonLib.JsonUtils
{
    public class FontJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Font));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is Font)
                return (Font)reader.Value;

            var fp = reader.Value.ToString().Split(',', ';');
            if (fp.Length > 0)
            {
                var ffam = fp[0].Trim();
                float fs = 8;
                if (fp.Length > 1)
                {
                    if (fp[1].EndsWith("pt"))
                        fp[1] = fp[1].Remove(fp[1].LastIndexOf("pt"));
                    if (!float.TryParse(fp[1].Replace('.', ',').Trim(), out fs))
                        fs = 8;
                }
                return new Font(ffam, fs);
            }
            else
                return null;

            //throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);            
            //throw new NotImplementedException();
        }
    }
}
