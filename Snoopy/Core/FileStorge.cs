using System;
using System.IO;
using LogHelper;
using CommonLib.Extentions.String;
using System.Threading;

namespace Snoopy.Core
{
	public class FileStorge
	{
		private int _maxTry = 10;

		public event EventHandler<Exception> FileException = delegate (object sender, Exception e) { };

		private IConverter _converter { get; set; }

		public string Path { get; private set; }
		
		private FileStorge(IConverter converter, string path, int maxTry)
		{
			_converter = converter;
			_maxTry = maxTry;
			if (!path.isVoid())
			{
				Path = path;
			}
			else
			{ 
				try
				{
					Path = AppDomain.CurrentDomain.BaseDirectory;					
				}
				catch (Exception ex)
				{
					Log.Write(ex, "FileStorge");
					throw ex;
				}
			}
		}

        /// <summary>
        /// Если converter не задан то работает только со строками
        /// Если initpath не задан то использует текущую директорию (директорию сборки)
        /// </summary>		
        public static FileStorge Create(IConverter converter, string initpath = "", int maxTry = 10)
		{
			if (converter == null || (!initpath.isVoid() && !Directory.Exists(initpath)))
				return null;
			return new FileStorge(converter, initpath, maxTry);
		}

		public static FileStorge Create(string initpath = "", int maxTry = 10)
		{
			if (!initpath.isVoid() && !Directory.Exists(initpath))
				return null;
			return new FileStorge(null, initpath, maxTry);
		}

		/// <summary>
		/// Если path -  только имя то выдаёт путь+имя
		/// иначе выдаёт тот же path
		/// </summary>
		public string FullPath(string path)
		{
			if (System.IO.Path.GetDirectoryName(path) == "")
				return this.Path + path;
			else
				return path;
		}

        public static string FileExt(string path)
            => System.IO.Path.GetExtension(path);
        
            public static string FileName(string path, bool withoutExt=false)
		{
			if (path == "") return "";
			string result = path;
			if (System.IO.Path.GetFullPath(path) != "") 
				result = System.IO.Path.GetFileName(path);
			if (withoutExt)
				return System.IO.Path.GetFileNameWithoutExtension(result);
			else
				return result;
		}


		#region TRY
		/// <summary>
		/// Делает maxTry попыток (с перехватом IOException) выполнения делегата RWOperation
		/// </summary>
		/// <param name="path"> имя файла в директории Path или полный путь к файлу</param>
		/// <param name="RWOperation">делегат Func: 1 входной string, 1 выходной string</param>
		/// <returns></returns>
		private string TryIO(string path, Func<string, string> RWOperation)
		{
			int itry = _maxTry;
			while (true)
			{
				try
				{
					return RWOperation(FullPath(path));
				}
				//catch (IOException)
				catch (Exception ex)
				{//попробуем еще раз
					if (ex is IOException && itry > 0)
					{
						itry--;
						continue;
					}
					else
					{
						throw ex;
					}
						
				}
			}
		}

        public static bool Exists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Чтение текстового файла
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string TryReadText(string name)			
		{
			try
			{ 
				return TryIO(name, (name_) => File.ReadAllText(name_));
			}
			catch (Exception ex)
			{
				Log.Write(ex, "TryReadText");
				return null;
			}

		}

		private bool TryWriteText(string name, string text)
		{
			try
			{
				TryIO(name, (name_) => { File.WriteAllText(name_, text); return text; });
				return true;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "TryWriteText");
				return false;
			}
		}

		private bool TryDelete(string name)
		{
			try
			{
				TryIO(name, (name_) => { File.Delete(name_); return ""; });
				return true;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "TryDelete");
				return false;
			}
		}

		public bool ReName(string name, string newName)
		{
			try
			{
				File.Move(FullPath(name), FullPath(newName));
				return true;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "ReName");
				return false;
			}
			
		}

		#endregion

		/// <summary>
		/// Загрузка файла в string (используя maxTry попыток)
		/// </summary>
		/// <param name="path">имя файла в директории Path</param>		
		public T Load<T>(string name)
		{
			if (_converter == null ) return default(T);
			else return _converter.Deserialize<T>(TryReadText(name));
		}

        public T Load<T>(string name, object id, CancellationToken cancellationToken, Action<object, int, string> onProgress)
        {
            if (_converter == null) return default(T);
            cancellationToken.ThrowIfCancellationRequested();
            onProgress?.Invoke(id, 50, "50%");
            var text = TryReadText(name);
            cancellationToken.ThrowIfCancellationRequested();
            onProgress?.Invoke(id, 100, "100%");
            var result = _converter.Deserialize<T>(text);
            return result;
        }

        public static DateTime LastWriteTime(string name)
		{
			return File.GetLastWriteTime(name);
		}

		/// <summary>
		/// Загружает файл, и если есть конвертерер - десериализует
		/// иначе просто возвращает строку
		/// </summary>
		/// <param name="name">имя файла, с путём или без</param>
		/// <returns>string или произвольный объект</returns>
		public object Load(string name)
		{
			return _converter?.Deserialize(TryReadText(name))?? TryReadText(name);
		}

		/// <summary>
		/// Сохраняет файл, если есть конвертерер - сериализует
		/// иначе просто сохраняет как строку (используя ToString)
		/// </summary>
		/// <param name="name">имя файла, с путём или без</param>
		/// <param name="obj">объект для сериализации или string если конвертер не задан</param>
		public bool Save(string name, object obj)
		{
			if (_converter == null)
				return TryWriteText(name, obj.ToString());
			else
				return TryWriteText(name, _converter.Serialize(obj));
		}

		public bool Delete(string name)
		{
			return TryDelete(name);
		}

		/// <summary>
		/// Извлекает корневой путь из содержимого файла
		/// </summary>
		//public string ReadIndexRootPath(string fileName)
		//{
		//	var reader = File.OpenRead(fileName);
		//	var buf= new byte[512];
		//	reader.Read(buf, 0, 512);
		//	string text = System.Text.Encoding.UTF8.GetString(buf);
		//	var result = "";
		//	int b = text.IndexOf("\"RootPath\"");
		//	if (b >= 0)
		//	{
		//		b = text.IndexOf(":\"", b + 10);
		//		if (b >= 0)
		//		{
		//			int e = text.IndexOf("\"", b + 2);
		//			if (e >= 0)
		//				result = text.Substring(b, e - b + 1);
		//		}
		//	}			
		//	reader.Close();
		//	return result;

		//	//throw new NotImplementedException();
		//}
	}
}