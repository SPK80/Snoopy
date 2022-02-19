using Newtonsoft.Json;
using CommonLib.Extentions.String;
using System;

namespace Snoopy.Core
{
    public interface IConverter
    {
        object Deserialize(string str);
        T Deserialize<T>(string str);
        string Serialize(object obj);
    }

    public class JSONConverter:IConverter
	{
        private readonly Formatting formatting;
        private readonly JsonSerializerSettings settings;

        public JSONConverter(bool indentedFormatting=false, JsonSerializerSettings settings=null)
        {
            if (indentedFormatting)
                formatting = Formatting.Indented;
            else
                formatting = Formatting.None;
            this.settings = settings;
        }

        public T Deserialize<T>(string str)
		{
			if (str.isVoid())
				return default(T);
			else
				return JsonConvert.DeserializeObject<T>(str);
		}

		public object Deserialize(string str)
		{
			if (str.isVoid())
				return null;
			else
				return JsonConvert.DeserializeObject(str);			
		}

		public string Serialize(object obj)
		{
            if (obj == null) return "";
            else return JsonConvert.SerializeObject(obj, formatting, settings);
        }
	}
}