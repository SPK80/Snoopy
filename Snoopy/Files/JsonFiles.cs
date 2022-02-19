using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snoopy.Files
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
            catch
            {
                return null;
            }
        }

        public static bool Write(string path, object obj, Formatting formatting = Formatting.None, JsonSerializerSettings settings = null)
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
