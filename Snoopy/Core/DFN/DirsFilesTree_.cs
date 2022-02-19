using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IndFin.Core.DFN.tmp

{
	public class DirsFilesTree : IIndex
	{
		[JsonIgnore] // Name не сохраняется в тело файла, только как имя файла!
		public string Name { get; private set;}

		public string RootPath { get; set; }
		public DateTime CreationTime { get; set; }
		public EnumerableDFNode RootNode { get; set; }
		public int Count { get => RootNode.Count(); }
		public bool IsEmpty { get => RootNode == null; }
		public DirsFilesTree(){}

		public DirsFilesTree(string name)
		{
			RootNode = null;
			RootPath = "";
			CreationTime = DateTime.Now;
			Name = name;
		}

		public DirsFilesTree(string rootPath, IIndexItem rootNode, string name)
		{
			RootNode = (EnumerableDFNode)rootNode;
			RootPath = rootPath;
			CreationTime = DateTime.Now;
			Name = name;
		}

		public bool Save(IObjectStorge storage, string newName="")
		{
			if (newName != "")
				if (storage.Save(newName, this))
				{
					Name = newName;
					return true;
				}
				else return false;
			else
				return (storage.Save(Name, this));
		}

		public bool ReName(IObjectStorge storage, string newName)
		{
			if (RootNode == null) return false;
			if (storage.ReName(Name, newName))
			{
				Name = newName;
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Фабричный метод.
		/// Создаёт новый объект из загруженн
		/// </summary>
		/// <param name="indexName"></param>
		/// <param name="storage"></param>
		/// <returns></returns>
		public static IIndex LoadNew(string indexName, IObjectStorge storage)
		{
			var newTree = storage.Load<DirsFilesTree>(indexName);
			newTree.Name = storage.FileName(indexName);//имя берем из имени файла
			return newTree;
		}

		public static Task<IIndex> LoadNewAsync(string indexName, IObjectStorge storage)
		{
			return Task<IIndex>.Factory.StartNew(() => LoadNew(indexName, storage));
		}

		//public static BackgroundWorker LoadNewAsync(string indexName, IObjectStorge storage,
		//	RunWorkerCompletedEventHandler workCompleted,
		//	ProgressChangedEventHandler progressChanged)
		//{
		//	var scanner = new BackgroundWorker();
		//	scanner.WorkerReportsProgress = true;
		//	scanner.WorkerSupportsCancellation = true;

		//	scanner.ProgressChanged += progressChanged;
		//	scanner.RunWorkerCompleted += workCompleted;

		//	scanner.DoWork += (sender, e) =>
		//	{

		//	};

		//	scanner.RunWorkerAsync(indexName);

		//	return scanner;
		//}

		public IEnumerable<DFResult> Query(string query, bool incDirs)
		{						
			var found = RootNode.Where(
				s => s.Data.Name.Contains(query) && (!s.IsDir() || incDirs)
				);
			foreach (var item in found)
			{
				var p = this.RootPath + item?.Directory() ?? "";
				yield return new DFResult(item.Data, p);
			}
		}

		/// <summary>
		/// Обновление дерева по результатам пересканирования
		/// </summary>
		public void RefreshData(IIndexItem newData)
		{
			RootNode = newData as EnumerableDFNode;
		}


		public BackgroundWorker Load(string indexName,
			RunWorkerCompletedEventHandler workCompleted,
			ProgressChangedEventHandler progressChanged,
			IObjectStorge storage)
		{
			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;

			worker.ProgressChanged += progressChanged;
			worker.RunWorkerCompleted += workCompleted;

			worker.DoWork += (sender, e) =>
			{
				if (worker.WorkerReportsProgress)
					worker.ReportProgress(0);

				if (worker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}
				var index = LoadNew(indexName, storage);

				if (worker.WorkerReportsProgress)
					worker.ReportProgress(100);

				e.Result = index;
			};

			worker.RunWorkerAsync(indexName);

			return worker;
		}
	}
}