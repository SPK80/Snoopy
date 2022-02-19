using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bindings
{
    public static class ReflectionExtentions
    {
        public static bool SetPropertyValue(this object obj, string propertyName, object value)
        {
            try
            {
                var tO = obj.GetType();
                var propertyInfo = tO.GetProperty(propertyName);
                propertyInfo.SetValue(obj, value);
                return true;
            }
            catch { return false; }
        }

        public static bool GetPropertyValue(this object obj, string propertyName, out object value)
        {
            value = null;
            try
            {
                var tO = obj.GetType();
                var propertyInfo = tO.GetProperty(propertyName);
                value = propertyInfo.GetValue(obj);
                return true;
            }
            catch { return false; }
        }
    }
}
