using System;
using System.Collections.Generic;
using IndFin.Config;

namespace IndFin.tmp2
{
	internal class ColSett
	{
		public string Caption { get; set; }
		public bool Visible { get; set; }

		public ColSett(string caption, bool visible)
		{
			Caption = caption ?? throw new ArgumentNullException(nameof(caption));
			Visible = visible;
		}
	}

	//internal enum ChangedField { Name, Visible }
	/// <summary>
	/// хранит настройки колонов и обеспечивает обновление через делегат _onChange
	/// </summary>	
	internal class ColSettings :Dictionary<string, ColSett>
	{
		public static ColSettings CreateWithDefaults()
		{
			return new ColSettings
			{
				{ FieldNamse.Name, new ColSett("Имя", true) },
				{ FieldNamse.Length, new ColSett("Размер", true) },
				{ FieldNamse.CreationTime, new ColSett("Создан", true) },
				{ FieldNamse.LastWriteTime, new ColSett("Изменён", true) },
				{ FieldNamse.LastAccessTime, new ColSett("Открыт", true) },
				{ FieldNamse.Path, new ColSett("Путь", true) }
			};
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
}