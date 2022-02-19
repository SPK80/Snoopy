using CommonLib.LogHelper;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Bindings
{
    public class BindedProperty
    {
        private readonly object obj;
        private PropertyInfo propertyInfo;
        public event EventHandler<ValueEventArgs> ValueChanging;
        public string ObjectName { get; private set; }
        public string Name { get => propertyInfo.Name; }

        private object convertToPropertyType(object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (value.GetType() != propertyType)
            //если типы не совпадают то пытаемся конвертировать :
            {
                TypeConverter tc = TypeDescriptor.GetConverter(propertyType);
                try
                {
                    return tc.ConvertFromString(value.ToString());
                }
                catch (Exception ex)
                {
                    Log.Write(ex, $"{this.ToString()}.Value set({value.ToString()})");
                    return null;
                }
            }
            else return value;
        }

        public object Value
        {
            get
            {
                var propertyType = propertyInfo.PropertyType;
                TypeConverter tc = TypeDescriptor.GetConverter(propertyType);
                try
                {
                    //return tc.ConvertToString(propertyInfo.GetValue(obj));
                    return propertyInfo.GetValue(obj);
                }
                catch (Exception ex)
                {
                    Log.Write(ex, $"{this.ToString()}.Value get({propertyInfo.ToString()})");
                    return null;
                }                
            }
            set
            {       
                ValueEventArgs vea = null;
                //попытка вызова события ValueChanging
                var val = convertToPropertyType(value); //конвертируем в тип свойства
                if (val == null) return; //конвертация неудачна
                ValueChanging?.Invoke(this, vea = new ValueEventArgs { Value = val, Cancellation = false });

                if (!(vea?.Cancellation??false)) 
                    propertyInfo.SetValue(obj, val); 
                //var propertyType = propertyInfo.PropertyType;
                //if (value.GetType() == propertyType) //типы совпадают 
                //{
                //    propertyInfo.SetValue(obj, value);
                //    return;
                //}
                ////если типы не совпадают то пытаемся конвертировать :
                //TypeConverter tc = TypeDescriptor.GetConverter(propertyType);
                //try
                //{
                //    propertyInfo.SetValue(obj, tc.ConvertFromString(value.ToString()));
                //}
                //catch(Exception ex)
                //{
                //    Log.Write(ex, $"{this.ToString()}.Value set({value.ToString()})");
                //}                
            }
        }

        public Type GetPropertyType() => propertyInfo.PropertyType;
        public object GetObject() => obj;

        public BindedProperty(object obj, PropertyInfo propertyInfo, string objectName="")
        {
            this.obj = obj ?? throw new ArgumentNullException(nameof(obj));
            this.ObjectName = objectName ?? throw new ArgumentNullException(nameof(objectName));
            if (ObjectName == "") ObjectName = obj.ToString();
            this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
        public BindedProperty(object obj, string propertyName)
        {
            this.obj = obj ?? throw new ArgumentNullException(nameof(obj));
            var tO = obj.GetType();
            propertyInfo = tO.GetProperty(propertyName);
        }       
    }

    public class ValueEventArgs: EventArgs
    {
        public object Value=null;
        public bool Cancellation=false;
    }

}