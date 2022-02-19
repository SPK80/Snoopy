using System;
using System.Collections.Generic;

namespace Snoopy.Views
{
    public interface ISettingsView
    {
        //Type GetSettingType(string catalog, string name);

        /// <summary>
        /// получаем(на View) простое значение (string, int, Color, Font и т.п.)
        /// </summary>
        //bool SetSettingValue(string catalog, string name, object value);

        /// <summary>
        /// получаем(на View) массив значений
        /// </summary>
        //bool SetSettingsArray(string catalog, string name, object[] values);

        /// <summary>
        /// получаем(на View) Dictionary значений
        /// </summary>
        //bool SetSettingsDictionary(string catalog, string name, Dictionary<string, object> values);
        
        /// <summary>
        /// сигнал о готовности инициализировать
        /// </summary>
        //event Action OnInitSettings;

        //event Action <Dictionary<string, Dictionary<string, object>>> OnSettingsChanged;

        /// <summary>
        /// передаём(на View) простое изменённое значение
        /// </summary>
        //            catalog name    values
        event Action <string, object> OnSettingChanged;
        //event Action <string, string, object[]> OnSettingArrayChanged;              // передаём(на View) изменённый массив значений
        //event Action <string, string, Dictionary<string, object>> OnSettingDictionaryChanged;//передаём(на View) изменённый Dictionary значений

        event Func<string, Type, object> GetSetting; //(string catalog, string name, Type type);


    }
}
