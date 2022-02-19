using System;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

namespace CommonLib.JsonUtils
{
    public class ColorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Color));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int getInt(string s)
            {
                int result = 0;
                int.TryParse(s, out result);
                return result;
            }
            if (reader.Value is Color)
                return (Color)reader.Value;

            var rgba = reader.Value.ToString().Split(',', ';').Select(s => getInt(s)).ToArray();
            if (rgba.Count() == 3) //RGB
            {
                return Color.FromArgb(rgba[0], rgba[1], rgba[2]);
            }
            else if (rgba.Count() == 4) //RGBA
            {
                return Color.FromArgb(rgba[0], rgba[1], rgba[2], rgba[3]);
            }
            else
                return Color.FromName(reader.Value.ToString());

            //throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
            //throw new NotImplementedException();
        }
    }
}
