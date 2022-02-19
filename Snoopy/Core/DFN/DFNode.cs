using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.IO;
using Newtonsoft.Json;

namespace Snoopy.Core.DFN
{    
    public abstract class DFNode
    {
		// Имя файла
		[JsonProperty("NM", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }
		//	Дата и время создания файла
		[JsonProperty("CT", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? CreationTime { get; set; }
		//	Дата и время изменения файла
		[JsonProperty("WT", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? LastWriteTime { get; set; }
		//	Дата и время олкрытия файла
		[JsonProperty("AT", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? LastAccessTime { get; set; }

		protected int Level { get => Parent != null ? Parent.Level + 1 : 0; }
		
		protected DirNode Parent { get; set; }
		/// <summary>
		/// Рекурсивное построение пути к файлу
		/// </summary>		
		public string FilePath(string delimiter = "\\")
		{
			if (Level > 0)
				return Parent.FilePath(delimiter) + delimiter + Name;
			else return "";
		}

		/// <summary>
		/// Рекурсивное построение пути к директории
		/// </summary>	
		public string DirectoryPath(string delimiter = "\\")
		{
			return Parent?.FilePath(delimiter) ?? "";
		}

		public DFNode FindRoot()
		{
			var n = this;
			while (n.Parent != null)
			{ n = n.Parent; }
			return n;
		}

		public class DFNodeCollection<T> : Collection<T>
			where T: DFNode
		{
			private readonly DirNode owner;

			public DFNodeCollection() { }

			public DFNodeCollection(DirNode owner)
			{
				this.owner = owner;
			}

			/// <summary>
			/// Вставка с проверкой уникальности item
			/// </summary>
			protected override void InsertItem(int index, T item)
			{
				if (!this.Contains(item))
				{
					base.InsertItem(index, item);
					item.Parent = owner;
				}
			}

			/// <summary>
			/// Удаление с разрывам связи с Parent
			/// </summary>
			protected override void RemoveItem(int index)
			{
				this[index].Parent = null;
				base.RemoveItem(index);
			}
		}
	}

	[JsonObject]
	public class FileNode: DFNode
	{
		//public DFData Data { get; set; }

		// Размер файла
		[JsonProperty("L", NullValueHandling = NullValueHandling.Ignore)]
		public long? Length { get; set; }

		public FileNode() : base() { }

		public FileNode(DFData fi) : this()
		{
			Name = fi.Name;
			Length = fi.Length;
			CreationTime = fi.CreationTime;
			LastWriteTime = fi.LastWriteTime;
			LastAccessTime = fi.LastAccessTime;
		}			
	}

	[JsonObject]
	public class DirNode : DFNode, IEnumerable
	{
		public DirNode() : base() {
			Files = new DFNodeCollection<FileNode>(this);
			Dirs = new DFNodeCollection<DirNode>(this);
		}

		public DirNode(DFData di):this()
		{			
			Name = di.Name;
			CreationTime = di.CreationTime;
			LastWriteTime = di.LastWriteTime;
			LastAccessTime = di.LastAccessTime;
		}
		
		[JsonProperty("FS", NullValueHandling = NullValueHandling.Ignore)]
		public DFNodeCollection<FileNode> Files { get; private set; }
		[JsonProperty("DS", NullValueHandling = NullValueHandling.Ignore)]
		public DFNodeCollection<DirNode> Dirs { get; private set; }

		/// <summary>
		/// уничтожает все пустые коллекции
		/// </summary>
		public void CleanVoids()
		{
			if (Dirs != null && Dirs.Count == 0)
				Dirs = null;
			if (Files != null && Files.Count == 0)
				Files = null;
		}

		public FileNode AddFile(DFData fi)
		{
			var child = new FileNode(fi);
			if (Files == null)
				Files = new DFNodeCollection<FileNode>(this);
			Files.Add(child);
			return child;
		}

		public DirNode AddDir(DFData di)
		{
			var child = new DirNode(di);
			if (Dirs == null)
				Dirs = new DFNodeCollection<DirNode>(this);
			Dirs.Add(child);
			return child;
		}

		public IEnumerator GetEnumerator()
		{
			//Инициализация стека FILO
			var nodes = new Stack<DirNode>(20);//начальный размер нада подобрать
			var rootNode = FindRoot();

			nodes.Push(rootNode as DirNode);
			//Обработка начальной папки
			yield return rootNode;
			while (nodes.Count > 0)
			{				
				var curNode = nodes.Pop();//получаем текущую папки из стека
				if (curNode.Files != null)
					foreach (var node in curNode.Files)
					{
						yield return node;
					}
				if (curNode.Dirs != null)
					foreach (var node in curNode.Dirs)
					{					
						nodes.Push(node);
						yield return node;
					}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();		
	}

}