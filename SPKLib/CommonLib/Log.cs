using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace CommonLib.LogHelper
{



    public class Log
	{
		private static object sync = new object();

		private static void _Write(string fullText)
		{
			try
			{
				// Путь .\\Log
				var pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
				if (!Directory.Exists(pathToLog))
					Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
				var filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
					AppDomain.CurrentDomain.FriendlyName, DateTime.Now));

				lock (sync)
				{
					File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
				}
			}
			catch
			{
				// Перехватываем все и ничего не делаем
			}
		}

		public static void Write(string msg)
		{
			_Write(string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] {1}\r\n", DateTime.Now, msg));
		}


		public static void Write(Exception ex, string comment="")
		{
			_Write(string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n",
				DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message+comment));
		}
	}
}

