using System;
using System.Linq;
using System.Collections.Generic;
using CommonLib.LogHelper;

namespace CommonLib.Config
{

    //public interface ISettings
    //{
    //    ISettingsCatalog GetCatalog(string name);
    //    bool Load();
    //    bool Save();
    //}

    /// <summary>
    /// 1: Хранит/генерит/предоставляет каталоги ISettingsCatalog
    /// 2: Загружает/Сохраняет в файл path
    /// </summary>
    public class SettingsManager//: ISettings
    {
        //private List<SettingsCatalog> catalogs { get; } = new List<SettingsCatalog>();
        private Dictionary<string, ISettingsCatalog> catalogs { get; } = new Dictionary<string, ISettingsCatalog>();

        private IConfigStorge storge;

        public SettingsManager(IConfigStorge configStorge)
        {
            storge = configStorge ?? throw new ArgumentNullException(nameof(configStorge));
        }

        public ISettingsCatalog GetCatalog(string name)
        {
            try
            {
                return catalogs[name];
            }
            catch
            {
                var catalog = new SettingsCatalog();
                catalogs.Add(name, catalog);
                return catalog;
            }
        }

        //internal class DicDic : Dictionary<string, Dictionary<string, string>> { }

        public bool Load()
        {
            Dictionary<string, Dictionary<string, string>> loaded =null;
            try
            {
                loaded = storge.Load();//Read<DicDic>(path);
                if (loaded == null) return false;
            }
            catch (Exception e)
            {
                Log.Write($"{catalogs.ToString()} : ошибка загрузки!");
                return false;
            }

            catalogs.Clear();                    
            foreach (var catalogKV in loaded)
            {
                //if (catalogs.Find(c => c.Name == catalogKV.Key) != default(SettingsCatalog))
                if (!catalogs.ContainsKey(catalogKV.Key))
                    catalogs.Add(catalogKV.Key, new SettingsCatalog(catalogKV.Value));
                //throw new Exception($"Ошибка в файле. Каталог {catalogKV.Key} был создан ранее.");
            }
            return true;
        }

        public bool Save()
        {
            if (catalogs == null || catalogs.Count < 1) return false;
            
            try
            {
                var dd = new Dictionary<string, Dictionary<string, string>>();
                foreach (var c in catalogs)
                {
                    dd.Add(c.Key, c.Value.ToDictionary(kv => kv.Key, kv => kv.Value));
                }
                return storge.Save(dd);//Write(path, catalogs, Formatting.Indented);
            }
            catch
            {
                Log.Write($"{catalogs.ToString()} : ошибка сохранения!");
                return false;
            }
        }
    }
    
    public interface IConfigStorge
    {
        bool Save(Dictionary<string, Dictionary<string, string>> catalogs);
        Dictionary<string, Dictionary<string, string>> Load();
    }
}
