using Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snoopy.Views
{
    public static class PropertiesAdapter
    {
        public struct UIRecord
        {
            public string ObjectName;
            public object PropertyValue;

            public UIRecord(string objectName, object propertyValue)
            {
                ObjectName = objectName ?? throw new ArgumentNullException(nameof(objectName));
                PropertyValue = propertyValue ?? throw new ArgumentNullException(nameof(propertyValue));
            }
        }

        public static Dictionary<string, string> GetUIDictionary(this IEnumerable<BindedProperty> bindedProperties, 
            string propertyName)
        {
            var result = new Dictionary<string, string>();
            foreach (var prop in bindedProperties.Where(p => p.Name == propertyName))
            {
                result.Add(prop.ObjectName, prop.Value.ToString());
            }
            return result;
        }

        public static void InitUIProperty(this IEnumerable<BindedProperty> bindedProperties, 
            string propertyName, string objectName, object value)
        {
            foreach (var prop in bindedProperties.Where(p => p.ObjectName == objectName && p.Name == propertyName))
            {
                prop.Value = value;
            }            
        }

        public static void InitUIProperties(this IEnumerable<BindedProperty> bindedProperties,
            string propertyName, IEnumerable<UIRecord> uIRecords)
        {
            foreach (var uiRec in uIRecords)
            {
                bindedProperties.InitUIProperty(propertyName, uiRec.ObjectName, uiRec.PropertyValue);
            }

        }

    }
}
