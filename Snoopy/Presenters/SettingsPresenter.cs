using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snoopy.Core;
using Snoopy.Files;
using Snoopy.Views;
using CommonLib.LogHelper;

namespace Snoopy.Presenters
{
    public class SettingsPresenter          //singleton
    {
        private static SettingsPresenter instanceHolder = null;
        
        public static SettingsPresenter Create(ISettingsView uiSettingsView, string name)
        {
            if (instanceHolder == null)
            {
                instanceHolder = new SettingsPresenter(uiSettingsView);
                instanceHolder.InitCatalogs(ViewCatalogName, CoreCatalogName);
                if (!instanceHolder.Load(name))
                {
                    Log.Write($"Ошибка загрузки {name}");
                }

                return instanceHolder;
            }
                
            else
                throw new Exception($"{nameof(SettingsPresenter)} уже был инициирован!");
        }

        public const string ViewCatalogName = "View";
        public const string CoreCatalogName = "Core";


        private SettingsPresenter(ISettingsView uiSettingsView)
        {
            this.uiSettingsView = uiSettingsView ?? throw new ArgumentNullException(nameof(uiSettingsView));
            settings = new Dictionary<string, Dictionary<string, object>>();

            this.uiSettingsView.OnSettingChanged +=(name,  value)=> SetSetting(ViewCatalogName, name, value);
            this.uiSettingsView.GetSetting += (name, type) => GetSetting(ViewCatalogName, name, type);

            //this.uiSettingsView.OnInitSettings += () =>
            //{
            //    foreach(var catalog in settings)
            //    {
            //        foreach (var setting in catalog.Value)
            //        {
            //            var t = setting.Value.GetType();
            //            this.uiSettingsView.SetSettingValue(catalog.Key, setting.Key, setting.Value);
            //        }
            //    }
            //};

            //this.uiSettingsView.OnSettingsChanged += (settingsDic) => //SetValue(name,"UI", value);
            //{
            //    if (settingsDic != null)
            //        foreach (var catalog in settingsDic)
            //        {
            //            foreach (var setting in catalog.Value)
            //                SetValue(catalog.Key, setting.Key, setting.Value);
            //            //SetCatalog(catalog.Key, catalog.Value);
            //        }
            //};
        }

        private ISettingsView uiSettingsView;

        private Dictionary<string, Dictionary<string, object>> settings;

        private bool Load(string name)
        {
            var newSettings = JsonFile.Read<Dictionary<string, Dictionary<string, object>>>(name);            

            if (newSettings != null)
            {
                foreach (var newCatalog in newSettings)
                {
                    if (settings.ContainsKey(newCatalog.Key))
                    {
                        var catalog = settings[newCatalog.Key];//ссылка на сущ. каталог
                        foreach (var newSetting in newCatalog.Value) //итерация по загуженному каталогу
                        {
                            if (catalog.ContainsKey(newSetting.Key))

                                catalog[newSetting.Key] = newSetting.Value;
                            else
                                catalog.Add(newSetting.Key, newSetting.Value);
                        }                        
                    }
                }                
            }                
            return (newSettings != null);
        }

        public bool Save(string name)
        {
            return JsonFile.Write(name, settings, Formatting.Indented);
        }

        public T GetSetting<T>(string catalog, string name) where T: class
            => GetSetting(catalog, name, typeof(T)) as T;

        public object GetSetting(string catalog, string name, Type type)
        {
            object setting = null;
            try
            {
                setting = settings[catalog][name];
                if (setting.GetType() == type)
                    return setting;
                if (setting is JToken || setting is JObject|| setting is JArray)
                {                    
                        setting = JsonConvert.DeserializeObject(setting.ToString(), type); //, new FontJsonConverter(), new ColorJsonConverter()
                    //else
                    //    setting = JsonConvert.DeserializeObject(setting.ToString(), type);
                }
                //else if (setting is JToken)
                //    setting = (setting as JToken).ToObject(type); //переопределяем setting                    
                //else if (setting is JObject)
                //    setting = (setting as JObject).ToObject(type);
                //else if (setting is JArray)
                //    setting = (setting as JArray).ToObject(type);
                else
                    return null; //неизвестный науке зверь
                //всё ок
                settings[catalog][name] = setting; //переопределяем в settings
                return setting; //выводим
            }
            catch(Exception ex)
            { //setting не найден
                return null;
            }
        }

        private void InitCatalogs(params string[] catalogs)
        {
            catalogs.ToList().ForEach(c => settings.Add(c, new Dictionary<string, object>()));            
        }


        public void SetSetting(string catalog, string name, object value)
        {
            //if (!settings.ContainsKey(catalog)) throw new KeyNotFoundException($"catalog {catalog} не найден!"); //Каталог не найден
            settings[catalog][name]= value; //если name не находится то автоматически добавляется (фишка Dictionary)
        }

            //public Dictionary<string, object> GetCatalog(string catalogName)
            //{
            //    try
            //    {
            //        return settings.First(s => s.Key == catalogName).Value;
            //    }
            //    catch
            //    {
            //        return null;
            //    }
            //}

            //public void SetCatalog(string catalogName, Dictionary<string, object> catalogSettings)
            //{
            //    if (settings.Keys.Contains(catalogName))
            //        settings.Remove(catalogName);                
            //    settings.Add(catalogName, catalogSettings);
            //}

            //public object GetValue(string catalog, string name)
            //{
            //    try
            //    {
            //        return settings[catalog][name];
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.Log.Write(ex, $"GetValue {name} {catalog}");
            //        return null;
            //    }
            //    //throw new NotImplementedException();
            //}

            //public bool SetValue(string catalog, string name, object value)
            //{            
            //    Dictionary<string, object> setting=null;
            //    try
            //    {// находим каталог, если его нет то добавляем
            //        if (settings.Keys.Contains(catalog))
            //        {
            //            setting = settings[catalog];
            //        }                    
            //        else
            //        {
            //            setting = new Dictionary<string, object>();
            //            settings.Add(catalog, setting);
            //        }
            //        // находим настройку, если её нет то добавляем
            //        if (setting.Keys.Contains(name))
            //            setting[name] = value;                
            //        else
            //            setting.Add(name, value);

            //        return true;
            //    }
            //    catch(Exception ex)
            //    {
            //        LogHelper.Log.Write(ex, $"SetValue {name} {catalog} {value} ");
            //        return false;
            //    }
            //    //throw new NotImplementedException();
            //}

            //private List<T> jArrayToList<T>(JArray jArray)
            //{

            //    //var list = new List<T>();
            //    var list = jArray.Select(j => j.ToObject<T>()).ToList();
            //    //foreach (var j in jArray)
            //    //{
            //    //    list.Add(j.ToObject<T>());
            //    //}
            //    return list;
            //}

            //private void typing(ref Dictionary<string, Dictionary<string, object>> newSettings)
            //{
            //    foreach (var cat in newSettings)
            //    {
            //        foreach (var setting in cat.Value)
            //        {
            //            if (setting.Value is JArray)
            //            {
            //                var list = new List<object>();
            //                foreach(var o in setting.Value as JArray)
            //                {
            //                    list.Add(o);
            //                }
            //                setting.Value = list;
            //            }
            //        }
            //    }
            //}

        }
}
