using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonLib.Config
{
    public class SettingsCatalog : ISettingsCatalog
    {
        private Dictionary<string, string> items;
        private Dictionary<string, Action<string>> setters;

        public SettingsCatalog()
        {
            items = new Dictionary<string, string>();
            setters = new Dictionary<string, Action<string>>();
        }

        public SettingsCatalog(Dictionary<string, string> items)
        {
            this.items = items ?? new Dictionary<string, string>();
            setters = new Dictionary<string, Action<string>>();
        }

        public string this[string settingName]
        {
            get=> items.ContainsKey(settingName) ? items[settingName] : "";
            set
            {
                if (!items.ContainsKey(settingName))
                    items.Add(settingName, value);
                else
                    items[settingName] = value;
            }
            
        }

        public void Apply()
        {
            foreach (var KV in setters)
            {
                KV.Value(this[KV.Key]);
            }
        }

        public void Bind(string settingName, Action<string> settingSetter)
        {
            if (setters.ContainsKey(settingName))
                setters[settingName] = settingSetter;
            else
                setters.Add(settingName, settingSetter);
        }

        public void Disconnect()
        {
            setters.Clear();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
