using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Extentions
{
    public static class Properties
    {
        public static object GetValue(this object obj, string property)
        {
            var p = obj.GetType()?.GetProperty(property);
            return p?.GetValue(obj);
        }

        public static void SetValue(this object obj, string property, object value)
        {
            var p = obj.GetType()?.GetProperty(property);
            p?.SetValue(obj, value);
        }

    }
}
