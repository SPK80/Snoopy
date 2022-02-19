using System;
using System.Collections.Generic;
using Binders;
using Snoopy.Core;
using Snoopy.Files;
using Newtonsoft.Json;
using CommonLib.LogHelper;

namespace Snoopy.Config
{
	#region примеры
	//	var settingsManager = new SettingsManager("settingsManager", new FileStorge(new JSONConverter()));
	//	var section1 = settingsManager.TryNewSection("section1");
	//	section1.Bind("setting1", () => class1.setting1.ToString(), (s) => { class1.setting1 = int.Parse(s);
	#endregion
		
	using DicDic = Dictionary<string, Dictionary<string, dynamic>>;

	public class SettingsManager : List<Binders<dynamic>>
	{
        private string name;

		//private List<Section<dynamic>> sections;
		//private FileStorge fileStorge;		

		public SettingsManager(string name)
		{			
			this.name = name;
			//fileStorge = objStorge;
			//sections = new List<Section<dynamic>>();
		}

		/// <summary>
		/// Если секция с таким именем уже есть возвращает найденную
		/// иначе создаёт и возвращает новую
		/// </summary>		
		public IBinded<dynamic> Section(string name)
		{
			var sect = this.Find(x=>x.Name==name);
			if (sect==null) //секция name не найдена
			{ //создаём новую секцию
				sect = new Binders<dynamic>(name);
				this.Add(sect);
			}
			return sect;
		}

		/// <summary>
		/// Связывет внешний источник определяемый делегатами getter и setter 
		/// с опцией name в секции section
		/// </summary>
		/// <param name="section">имя секции</param>
		/// <param name="name">имя опции</param>
		/// <param name="getter">делегат: внешний объект => опция </param>
		/// <param name="setter">делегат: опция => внешний объект </param>
		/// <returns>секция содержащая связанную опцию</returns>
		public IBinded<dynamic> Bind(string section, string name, Func<dynamic> getter, Action<dynamic> setter)
		{	
			return Section(section).Bind(name, getter, setter);
		}

		public bool Load()
		{
			var dic = JsonFile.Read<DicDic>(name);
			if (dic != null)
			{ 
				foreach (var kv in dic)//цикл по секциям
				{
					var sect = this.Find(x => x.Name == kv.Key);
					if (sect != null)
						sect.FromDictionary(kv.Value); //kv.Value - секция в формате Dictionary<string, string>
				}
				return true;
			}
			else
			{
				Log.Write($"{this.ToString()} : {name} не загружен!");
				return false;
			}				
		}

		public bool Save()
		{
			var dic = new DicDic();
			if (this.Count<1) return false;
			foreach (var sect in this)
			{				
				dic.Add(sect.Name, sect.ToDictionary());
			};
			return JsonFile.Write(name, dic, Formatting.Indented);
		}

	}

}