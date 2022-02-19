using Newtonsoft.Json;

namespace CommonLib.JsonUtils
{
    public static class Json
    {
        public static string Serialize(object obj, bool indented=false)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, indented?Formatting.Indented: Formatting.None);
            }
            catch
            {
                return "";
            }

        }
    }

}
