using System;
using System.Collections.Generic;
using IndFin.Config;

namespace IndFin.tmp
{
	internal enum ChangedField { Name, Visible }
	/// <summary>
	/// хранит настройки колонов и обеспечивает обновление через делегат _onChange
	/// </summary>
	internal class ColSettings
	{
		public readonly Action<ColSettings, ChangedField> _onChange;
		private string _name;
		public string Name { get => _name; set { _name = value; _onChange?.Invoke(this, ChangedField.Name); } }
		private bool _visible;
		public bool Visible { get => _visible; set { _visible = value; _onChange?.Invoke(this, ChangedField.Visible); } }

		public ColSettings(string name, bool visible, Action<ColSettings, ChangedField> onChange)
		{
			Name = name;
			Visible = visible;
			_onChange = onChange;
		}
	}

	public static class FieldNamse
	{
		public const string Name = "Name";
		public const string Length = "Length";
		public const string CreationTime = "CreationTime";
		public const string LastWriteTime = "LastWriteTime";
		public const string LastAccessTime = "LastAccessTime";
		public const string Path = "Path";
	}

	/// <summary>
	/// 
	/// </summary>
	internal class ColSettingsList : List<ColSettings>
	{

		public bool DefVisible { get; set; }
		/// <summary>
		/// Инициализация коллекции по шаблону FieldNames
		/// </summary>
		/// <param name="setter">Делегат выполняемый при изменении любого свойства ColSettings</param>
		public ColSettingsList(Action<ColSettings, ChangedField> setter, bool visible=false)
		{
			Add(new ColSettings(FieldNamse.Name, visible, setter));
			Add(new ColSettings(FieldNamse.Length, visible, setter));
			Add(new ColSettings(FieldNamse.CreationTime, visible, setter));
			Add(new ColSettings(FieldNamse.LastWriteTime, visible, setter));
			Add(new ColSettings(FieldNamse.LastAccessTime, visible, setter));
			Add(new ColSettings(FieldNamse.Path, visible, setter));
		}

		/// <summary>
		/// Доступ к элементам по имени
		/// </summary>
		public ColSettings this[string name]
		{
			get => Find((cs) => cs.Name == name);
		}

		public string[] GetAllVisible()
		{
			var result = new List<string>();
			foreach (var cs in this)
			{
				if (cs.Visible)
					result.Add(cs.Name);
			}
			return result.ToArray();
		}				
	}

}