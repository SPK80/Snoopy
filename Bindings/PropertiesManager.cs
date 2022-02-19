using System;
using System.Collections.Generic;
using System.Linq;
//using DependencyInvertion;
using CollectionExtentions;

namespace Binders
{
    public class PropertiesManager
    {
        public Dictionary<object, List<Binder<dynamic>>> Properties { get; private set; }
            = new Dictionary<object, List<Binder<dynamic>>>();
            
        public void AddProperties(object obj, params string[] propertyNames)
        {
            foreach (var pn in propertyNames)
            {
                AddProperty(obj, pn);
            }
        }

        public bool AddProperty(object obj, string propertyName)
        {
            if (obj == null) return false;
            //проверка на уникальность
            bool objPresent = Properties.ContainsKey(obj);
            if (objPresent &&
                Properties[obj].Find(b => b.Name == propertyName) != null) return false;

            var type = obj.GetType();
            var pI = type?.GetProperty(propertyName);
            if (pI == null) return false;
            // если такого obj еще нет в Dictionary, то добавляем
            if (!objPresent)
                Properties.Add(obj, new List<Binder<dynamic>>());

            Properties[obj].Add(new Binder<dynamic>(propertyName,
                () => pI?.GetValue(obj),
                (p) => pI?.SetValue(obj, p)
                ));
            return true;
        }

        public bool RemoveProperty(object obj, string propertyName)
        {
            if (obj == null || !Properties.ContainsKey(obj) || Properties[obj] == null) return false;
            return Properties[obj].RemoveAll(b => b.Name == propertyName) > 0;
        }

        public bool RemoveObject(object obj)
        {
            //if (obj == null || !properties.ContainsKey(obj)) return false;
            return Properties.Remove(obj);
        }

        public void SetValues(string propertyName, Dictionary<object, dynamic> values)
        {
            foreach (var p in Properties)
            {
                if (values.ContainsKey(p.Key))
                {
                    var binder = p.Value?.First(b => b.Name == propertyName);
                    if (binder != null)
                        binder.Value = values[p.Key];
                }
            }
        }

        public bool SetValue(string propertyName, dynamic value, object obj)
        {
            if (obj == null || !Properties.ContainsKey(obj)) return false;
            try
            {
                Properties[obj].FirstOrDefault(p => p.Name == propertyName).Value = value;
                return true;
            }
            catch { return false; }
        }

        public bool SetValue(string propertyName, dynamic value)
        {
            if (Properties.Count < 1) return false;
            var result = true;
            foreach (var values in Properties)
            {
                result = result & SetValue(propertyName, value, values.Key);
            }
            return result;
        }

        public bool SetValue(dynamic value, string[] propertyNames, object[] objects=null)
        {
            if (propertyNames.IsVoid() || propertyNames.IsVoid()) return false;
            var result = true;
            
            if (objects.IsVoid())
            {
                foreach (var pn in propertyNames)
                {
                    result = result & SetValue(pn, value);
                }
            }
            else
            {
                foreach (var pn in propertyNames)
                {
                    foreach (var obj in objects)
                        result = result & SetValue(pn, value, obj);
                }
            }
            
            return result;
        }

        public Dictionary<object, dynamic> GetValues(string propertyName)
        {
            var result = new Dictionary<object, dynamic>();
            foreach (var p in Properties)
            {
                var binder = p.Value?.First(b => b.Name == propertyName);
                if (binder != null)
                    result.Add(p.Key, binder.Value);
            }
            return result;
        }
        
        public dynamic GetValue(object obj, string propertyName)
        {
            if (obj == null || !Properties.ContainsKey(obj)) return null;
            return Properties[obj].First(p => p.Name == propertyName)?.Value;
        }
                
    }
}