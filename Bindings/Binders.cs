using System;
using System.Collections.Generic;
using System.Linq;

namespace Binders
{
    public interface IBinded<T>
    {
        IBinded<T> Bind(string name, Func<T> getter, Action<T> setter);
    }

    public class Binder<T>
    {
        public static explicit operator T(Binder<T> binder)
        {
            return binder.Value;
        }

        public string Name { get; }
        //вызов геттера или сеттера при обращении к Value
        public T Value
        {
            get => getter();
            set => setters(value);
        }
        private event Func<T> getter = () => default(T);
        private event Action<T> setters = (x) => { };

        public void AddSubscriber(Action<T> setter)
        {
            if (setter == null) return;
            setters += setter;
        }
        public Binder(string key, Func<T> getter, Action<T> setter)
        {
            Name = key;
            Bind(getter, setter);
        }
        public void Bind(Func<T> getter, Action<T> setter)
        {
            if (getter != null)
                this.getter = getter;
            if (setter != null)
                this.setters = setter;
        }

        public KeyValuePair<string, T> ToKeyValuePair()
        {
            return new KeyValuePair<string, T>(Name, Value);
        }
    }

    public class Binders<T> : IBinded<T>
    {
        private List<Binder<T>> binders = new List<Binder<T>>();
        public Binders(string name) => Name = name;

        public string Name { get; protected set; }

        /// <summary>
        /// Добавляет новую уникальную связь
        /// </summary>
        public IBinded<T> Bind(string name, Func<T> getter, Action<T> setter)
        {
            var b = binders.Find(x => x.Name == name);
            if (b == null) //проверка на уникальность
                binders.Add(new Binder<T>(name, getter, setter));
            else
                b.AddSubscriber(setter);
            //	Log.Write($"{this.ToString()} : Опция {name} уже существует!");

            return this;
        }
                
        ///// <summary>
        /// Доступ к значениям по имени key
        /// </summary>		
        public T this[string key]
        {
            get
            {
                var kv = binders.Find(x => x.Name == key);
                if (kv == null)
                    return default(T);
                else
                    return binders.Find(x => x.Name == key).Value;
            }
            set
            {
                var kv = binders.Find(x => x.Name == key);
                if (kv != null)
                    kv.Value = value;
            }
        }
    
        public Dictionary<string, T> ToDictionary()
        {
            var dic = new Dictionary<string, T>();
            foreach (var binder in binders)
            {
                if (dic.ContainsKey(binder.Name))
                    dic[binder.Name] = binder.Value;
                else
                    dic.Add(binder.Name, binder.Value);

            };
            return dic;
        }

        public void FromDictionary(Dictionary<string, T> dic)
        {
            foreach (var kv in dic)
            {
                binders.Where(b => b.Name == kv.Key).ToList().ForEach(b => b.Value = kv.Value);
                //this[kv.Key] = kv.Value;
            }
        }

    }

    //class BindedProperty<T>
    //{
    //    private Func<T> getter;
    //    private Action<T> setter;

    //    //public string PropertyName { get; }

    //    public T Value
    //    {
    //        get => getter();
    //        set => setter(value);
    //    }

    //    public BindedProperty(Func<T> getter, Action<T> setter)
    //    {
    //        this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
    //        this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
    //        //PropertyName = name ?? throw new ArgumentNullException(nameof(name));
    //    }

    //}

    //class BindedObject 
    //{
    //    private Dictionary<string, BindedProperty<object>> properties=
    //        new Dictionary<string, BindedProperty<object>>();
    //    //public string ObjectName { get; }

    //    //public BindedObject(string Name, params BindedProperty<object>[] properties)
    //    //{
    //    //    ObjectName = Name ?? throw new ArgumentNullException(nameof(controlName));
    //    //    if (properties == null) throw new ArgumentNullException(nameof(properties));
    //    //    AddRange(properties);
    //    //}
    //    public object this[string key]
    //    {
    //        get => properties[key].Value;
    //        set => properties[key].Value=value;
    //    }
    //    public void Add(string propertyName, BindedProperty<object> bindedProperty)
    //        => properties.Add(propertyName, bindedProperty);
    //    public void Remove(string propertyName)
    //        => properties.Remove(propertyName);
    //    public void Clear(string propertyName)
    //        => properties.Clear();
    //}

    //public class PropertiesManager
    //{
    //    //private List<BindedObject> bindedObjects = new List<BindedObject>();
    //    private Dictionary<string, BindedObject> bindedObjects =
    //        new Dictionary<string, BindedObject>();

    //    public void AddProperty<T>(string objectName, string propertyName, Func<T> getter, Action<T> setter)
    //        where T : class
    //    {
    //        if (!bindedObjects.ContainsKey(objectName))
    //            bindedObjects.Add(objectName, new BindedObject());

    //        var obj = bindedObjects[objectName];
    //        obj.Add(propertyName, new BindedProperty<T>(getter, setter));

    //        //bindedObjects.FirstOrDefault(b => b.ObjectName == objectName);
    //    }

    //    public void SetValues<PropertyType>(string propertyName, Dictionary<string, PropertyType> values)
    //        where PropertyType : class
    //    {
    //        //foreach (var c in bindedObjects)
    //        //{
    //        foreach (var kv in values)
    //        {
    //            bindedObjects[kv.Key][propertyName] = kv.Value;
    //        }

    //        //var props = c.Value.Where(p => p.PropertyName == propertyName);
    //        //    foreach (var p in props)
    //        //    {
    //        //        //if (values.ContainsKey(c.ObjectName))
    //        //        //    p.Value = values[c.ObjectName];

    //        //    }
    //        //}
    //    }

    //    public Dictionary<string, PropertyType> GetValues<PropertyType>(string propertyName)
    //       where PropertyType : class
    //    {
    //        var result = new Dictionary<string, PropertyType>();

    //        foreach(var obj in bindedObjects)
    //        {
    //            var val = obj.Value[propertyName] as PropertyType;
    //            result.Add(obj.Key, val);
    //        }

    //        return result;
    //    }

    //}
}
