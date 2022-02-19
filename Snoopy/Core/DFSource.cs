using System;
using System.Collections.Generic;
using System.IO;
using CommonLib.LogHelper;

namespace Snoopy.Core
{
    static class FieldNames
    {
        public const string Name = nameof(FoundItem.Name);
        public const string Length = nameof(FoundItem.Length);
        public const string CreationTime = "CreationTime";
        public const string LastWriteTime = nameof(FoundItem.Updated);
        public const string LastAccessTime = "LastAccessTime";
        public const string Path = nameof(FoundItem.Path);
    }

    /// <summary>
    /// Источник файтов и каталогов
    /// </summary>
    public class DFSource
	{
		private event Action<Exception> ScanException = (e) => Log.Write(e, "ScanException");

		public string RootPath { get; }

		private List<string> processingFields;
		

		public DFSource(string rootPath = "", List<string> processingFields = null )
		{
			RootPath = rootPath;
			this.processingFields = processingFields;
		}

		private string fullPath(string path)
		{
			if (RootPath == "" || path.IndexOf(RootPath) == 0)//RootPath не задан или сождержится в начале path
				return path;
			else
				return RootPath + "\\" + path;
		}

		public string[] GetDirectories(string path)
		{
			return TryGetItems(fullPath(path), Directory.GetDirectories);							
		}

		public string[] GetFiles(string path)
		{
			return TryGetItems(fullPath(path), Directory.GetFiles);
		}

		private int processingFieldsCount => processingFields?.Count ?? 0;

		private DFData getDirData(string fpath)
		{
			var di = new DirectoryInfo(fpath);
            if (processingFieldsCount < 1)
                return new DFData
                {
                    Name = di.Name,
                    Length = null,
                    CreationTime = di.CreationTime,
                    LastWriteTime = di.LastWriteTime,
                    LastAccessTime = di.LastAccessTime
                };
            else
                return new DFData
                {
                    Name = processingFields.Contains(FieldNames.Name) ? di.Name : null,
                    Length = null,
                    CreationTime = (processingFields.Contains(FieldNames.CreationTime)) ? di.CreationTime : (DateTime?)null,
                    LastWriteTime = (processingFields.Contains(FieldNames.LastWriteTime)) ? di.LastWriteTime : (DateTime?)null,
                    LastAccessTime = (processingFields.Contains(FieldNames.LastAccessTime)) ? di.LastAccessTime : (DateTime?)null,
                };
		}

		private DFData getFileData(string fpath)
		{
			var fi = new FileInfo(fpath);
			if (processingFieldsCount < 1)
				return new DFData
				{
					Name = fi.Name,
					Length = fi.Length,
					CreationTime = fi.CreationTime,
					LastWriteTime = fi.LastWriteTime,
					LastAccessTime = fi.LastAccessTime
				};
			else
				return new DFData
				{
					Name = processingFields.Contains(FieldNames.Name) ? fi.Name : null,
					Length = processingFields.Contains(FieldNames.Length) ? fi.Length : (long?)null,
					CreationTime = (processingFields.Contains(FieldNames.CreationTime)) ? fi.CreationTime : (DateTime?)null,
					LastWriteTime = (processingFields.Contains(FieldNames.LastWriteTime)) ? fi.LastWriteTime : (DateTime?)null,
					LastAccessTime = (processingFields.Contains(FieldNames.LastAccessTime)) ? fi.LastAccessTime : (DateTime?)null,
				};
		}

		public DFData GetDFData(string path)
		{
			var fpath = fullPath(path);
			if (Directory.Exists(fpath))
			{
				return getDirData(fpath);
			}
			else if (File.Exists(fpath))
			{
				return getFileData(fpath);
			}
			else
				return null;
		}
		
		private string[] TryGetItems(string path, Func<string, string[]> getItems)
		{
			string[] items = null;
			//если path указывает на директорию,
			//то получаем список её файлов и субдиректорий
			if (Directory.Exists(path) && getItems != null)
			{
				try
				{ items = getItems(path); }
				catch (UnauthorizedAccessException e)
				{//нет доступа
					ScanException(e);
				}
				catch (DirectoryNotFoundException e)
				{//не найдено
					ScanException(e);
				}
			}
			return items;
		}

		public bool Exists(string path)
		{
			var fpath = fullPath(path);
			return Directory.Exists(fpath) || File.Exists(fpath);
		}

		public string ParentPath(string path)
		{
			return Path.GetDirectoryName(path);
		}
	}
}