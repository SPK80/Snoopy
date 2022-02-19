using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Binders;

namespace Snoopy
{
   
 //   public interface IConverter
	//{
	//	object Deserialize(string str);
	//	T Deserialize<T>(string str);
	//	string Serialize(object obj);
	//}

	//public interface IFileStorge
	//{
	//	string Path { get; } // определяет текущее хранилище (папку, сервер и пр.)
	//	T Load<T>(string name);   //name - имя файла, без пути
	//	object Load(string name);
 //       T Load<T>(string name, object id, CancellationToken cancellationToken, Action<object, int, string> onProgress);

 //       bool Save(string name, object obj);
	//	bool ReName(string name, string newName);
	//	bool Delete(string name);
	//	//string FileName(string path, bool withoutExt);
	//	string FullPath(string path);
	//	//DateTime LastWriteTime(string name);
	//}

	//public interface IDFData
	//{
	//	string Name { get; set; }
	//	long? Length { get; set; }
	//	DateTime? CreationTime { get; set; }
	//	DateTime? LastAccessTime { get; set; }
	//	DateTime? LastWriteTime { get; set; }
	//}

 //   public interface IDFResult
 //   {
 //       string Name { get; set; }
 //       long? Length { get; set; }
 //       DateTime? CreationTime { get; set; }
 //       DateTime? LastAccessTime { get; set; }
 //       DateTime? LastWriteTime { get; set; }
 //       string Path { get; set; }
 //       string DirPath();
 //       string FilePath();
 //   }

    //public interface IIndex
    //{
    //	string RootPath { get; }
    //	string Name { get; }
    //	DateTime CreationTime(IObjectStorge storage);
    //	bool IsEmpty { get; }
    //	int Count { get; }
    //	//BackgroundWorker Worker { get; }
    //	bool CancelProgress();

    //	IEnumerable<IDFResult> Query(string query, bool incDirs, bool parallel, char splitter = char.MinValue);

    //	bool Load(IObjectStorge storage, 
    //		Action<IIndex> onCompleted,
    //		Action<IIndex> onCancelled,
    //		Action<IIndex, int> progressChanged);

    //	bool Scan(IDFSource source,
    //		Action<IIndex> onCompleted,
    //		Action<IIndex> onCancelled,
    //		Action<IIndex, int, int> progressChanged);

    //	bool Save(IObjectStorge storage, string newName = "");

    //	bool ReName(IObjectStorge storage, string newName);
    //	//void RefreshData(IIndexItem newData);
    //}


    /// <summary>
    /// Декларирует доступ к информации о файлах и директориях
    /// по указанному пути path
    /// </summary>
 //   public interface IDFSource
	//{
	//	string RootPath { get; }
	//	/// <summary>
	//	/// проверяет доступность объекта указанного path
	//	/// </summary>
	//	/// <param name="path">путь к объекту</param>		
	//	bool Exists(string path);

	//	/// <summary>
	//	/// возвращает массив имён файлов в указанной path директории
	//	/// </summary>
	//	/// <param name="path">путь к директории</param>		
	//	string[] GetFiles(string path);

	//	/// <summary>
	//	/// возвращает массив имён директорий в указанной path родительской директории
	//	/// </summary>
	//	/// <param name="path">путь к родительской директории</param>
	//	string[] GetDirectories(string path);

	//	/// <summary>
	//	/// возвращает заполненную структуру с информацией об указанном в path объекте
	//	/// </summary>
	//	/// <param name="path">полный путь к объекту</param>
	//	IDFData GetDFData(string path);
	//}

	//public interface IExecuter
	//{
	//	//bool OpenFile(string fullPath);
	//	//bool OpenDir(string fullPath);
	//	bool ExecFile(string path);
	//	bool ShowInExplorer(string path);
	//}

	//public interface IIndexScanner
	//{
	//	//IIndexItem Scan(string rootPath, CancellationToken cancellationToken = default(CancellationToken));
	//	Task<IIndexItem> ScanTask(string rootPath);
	//	void Cancel();
	//}


	//public interface IBinded<T>
	//{
	//	IBinded<T> Bind(string name, Func<T> getter, Action<T> setter);
	//}


	//public interface ISettingsManager
	//{
	//	bool Load();
	//	bool Save();
	//	IBinded<dynamic> Section(string name);
	//	IBinded<dynamic> Bind(string section, string name, Func<dynamic> getter, Action<dynamic> setter);
	//}

}