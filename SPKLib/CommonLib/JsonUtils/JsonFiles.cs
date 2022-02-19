using System;
using System.IO;
using Newtonsoft.Json;

namespace CommonLib.JsonUtils
{
    public static class JsonFile
    {
        public static T Read<T>(string path) where T : class
        {
            if (path=="") return null;
            try
            {
                var text = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool Write(string path, object obj, Formatting formatting = Formatting.Indented, JsonSerializerSettings settings = null)
        {
            if (path == "") return false;
            try
            {
                var text = JsonConvert.SerializeObject(obj, formatting, settings);
                File.WriteAllText(path, text);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
