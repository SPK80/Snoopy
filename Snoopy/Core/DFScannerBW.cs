using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using IndFin.Core.DFN;

namespace IndFin.Core
{
	public class DFTreeScannerBW: BackgroundWorker
	{
		private IDFSource dirSource;
		public string RootPath { get; private set; }
		public IIndex index { get; private set; }


		public DFTreeScannerBW(IDFSource dirSource, IIndex index = null)
		{
			this.dirSource = dirSource;
			this.index = index;
		}

		private int AddChilds(EnumerableDFNode node, string[] names)
		{
			if (names == null) return 0;
			foreach (string name in names)
			{
				node.Children.Add(new EnumerableDFNode(dirSource.GetDFData(name)));
			}
			return names.Length;
		}

		public void Start(string rootPath)
		{
			RootPath = rootPath;
			RunWorkerAsync(rootPath);			
		}		

		protected override void OnDoWork(DoWorkEventArgs e)
		{			
			if (e?.Argument == null) return;
			e.Result = null;
			string rootPath = "";
			rootPath = (string)e.Argument;
			if (!dirSource.Exists(rootPath)) return;

			var pState = new ScanProcessSate(rootPath);
			//int counter = 0;
			var nodes = new Stack<EnumerableDFNode>(20); //стек узлов
			var result = new EnumerableDFNode(dirSource.GetDFData(rootPath)); //корневой узел
			nodes.Push(result);
			while (nodes.Count > 0)
			{
				if (CancellationPending)
				{
					e.Cancel = true;
					return;
				}

				var curNode = nodes.Pop(); //получение очередного узла для обработки

				var files = dirSource.GetFiles(rootPath + "\\" + curNode.FilePath()); //список путей к файлам
				if (files != null) //заполнение узла данными по списку файлов
				{
					pState.files += AddChilds(curNode, files);
				}

				var dirs = dirSource.GetDirectories(rootPath + "\\" + curNode.FilePath()); //список путей к директориям
				if (dirs != null)
				{
					pState.dirs += AddChilds(curNode, dirs);					

					foreach (var dn in curNode.Children)
					{
						nodes.Push(dn as EnumerableDFNode);
					}
				}
				if (WorkerReportsProgress)
					ReportProgress(0, pState);				
			}
			e.Result = result;
		}			
	}
}