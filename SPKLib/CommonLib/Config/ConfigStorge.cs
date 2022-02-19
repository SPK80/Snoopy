using System;
using System.Collections.Generic;
using CommonLib.Config;
using CommonLib.JsonUtils;

namespace CommonLib.Config
{
    public class ConfigStorge: IConfigStorge
    {
        private string configFile;

        public ConfigStorge(string configFile)
        {
            this.configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
        }

        public Dictionary<string, Dictionary<string, string>> Load()
        {
            return JsonFile.Read<Dictionary<string, Dictionary<string, string>>>(configFile);
        }

        public bool Save(Dictionary<string, Dictionary<string, string>> catalogs)
        {
            return JsonFile.Write(configFile, catalogs);            
        }

        //private Control[] controls;

        //public ConfigStorge(string configFile, params Control[] controls)
        //{
        //    this.configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
        //    this.controls = controls ?? throw new ArgumentNullException(nameof(controls));

        //}

        //public bool Load()
        //{
        //    try
        //    {
        //        var configs = System.IO.File.ReadAllLines(configFile);
        //        foreach (var s in configs)
        //        {
        //            controls.First(c=>c.Name.Contains("Login"))=
        //        }

        //        for (int i=0; i<controls.Length; i++)
        //        {

        //            controls[i].Name
        //            controls[i].Text = configs[i];
        //        }
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;

        //    }
        //}
        //public bool Save()
        //{

        //}
    }
}
