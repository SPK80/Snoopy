
using System;

namespace Snoopy.Core
{
	/// <summary>
	/// Хранит результат поиска
	/// </summary>
	//public struct DFResult : IDFResult
	//{
	//	/// Имя файла
	//	public string Name { get; set; }

	//	/// Размер файла
	//	public long? Length { get; set; }

	//	///	Дата и время создания файла
	//	public DateTime? CreationTime { get; set; }

	//	///	Дата и время изменения файла
	//	public DateTime? LastWriteTime { get; set; }

	//	///	Дата и время олкрытия файла
	//	public DateTime? LastAccessTime { get; set; }

	//	public string Path { get; set; }

	//	public string DirPath() => Path.TrimEnd('\\');

	//	public string FilePath() => DirPath() + "\\" + Name;

	//	//public DFResult() { }
	//	public static DFResult Create(DFData data, string path)
	//	{
	//		return new DFResult(data, path);		
	//	}

	//	public DFResult(DFData data, string path)
	//	{
	//		Name = data.Name;
	//		Length = data.Length;
	//		CreationTime = data.CreationTime;
	//		LastWriteTime = data.LastWriteTime;
	//		LastAccessTime = data.LastAccessTime;
	//		Path = path;
	//	}
	//}
}