using System;
using System.Collections.Generic;

namespace CommonLib.Config
{
    public interface ISettingsCatalog: IEnumerable<KeyValuePair<string, string>>
    {
        string this[string settingName] { get; set; }

        void Apply();
        
        void Bind(string settingName, Action<string> settingSetter);

        void Disconnect();
    }
    
}