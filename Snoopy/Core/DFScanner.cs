using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IndFin.Core.DFN;

namespace IndFin.Core
{
	public class DFTreeScanner : IIndexScanner
	{
		private IDFSource _dirSource;

		private event SendOrPostCallback _processChanged;

		private CancellationTokenSource _tokenSource;

		public void Cancel() => _tokenSource?.Cancel();

		public DFTreeScanner(IDFSource dirSource, SendOrPostCallback processChanged)
		{
			_dirSource = dirSource;
			_processChanged = processChanged;
			_tokenSource = null;
		}

		//private EnumerableDFNode[] AddChilds(EnumerableDFNode node, string[] names)
		//{
		//	if (names == null) return null;
		//	var nodes = new EnumerableDFNode[names.Length];			
		//	int i = 0;
		//	foreach (string name in names)
		//	{
		//		nodes[i] = new EnumerableDFNode(_dirSource.GetDFData(name));
		//		node.Children.Add(nodes[i]);
		//		i++;
		//	}
		//	return nodes;

		private int AddChilds(EnumerableDFNode node, string[] names)
		{
			if (names == null) return 0;
			foreach (string name in names)
			{
				node.Children.Add(new EnumerableDFNode(_dirSource.GetDFData(name)));
			}
			return names.Length;
		}

		/// <summary>
		/// Создаёт задачу сканирования.
		/// Используется для async/await
		/// </summary>
		public Task<IIndexItem> ScanTask(string rootPath)
		{
			_tokenSource = new CancellationTokenSource();
			var _cancellationToken = _tokenSource.Token;
			return Task<IIndexItem>.Factory.StartNew(() => Scan(rootPath, _cancellationToken), _cancellationToken);
		}

		/// <summary>
		/// Сканирует дерево каталогов начиная с rootPath
		/// cancellationToken используется для ScanTask - async/await
		/// </summary>
		private IIndexItem Scan(string rootPath, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!_dirSource.Exists(rootPath)) return null;
			var pState = new ScanProcessSate(rootPath);
			//int counter = 0;
			var nodes = new Stack<EnumerableDFNode>(20); //стек узлов
			var result = new EnumerableDFNode(_dirSource.GetDFData(rootPath)); //корневой узел
			nodes.Push(result);
			while (nodes.Count > 0)
			{
				if (cancellationToken != default(CancellationToken))
					cancellationToken.ThrowIfCancellationRequested(); //отмена поторка по запросу

				var curNode = nodes.Pop(); //получение очередного узла для обработки

				var files = _dirSource.GetFiles(rootPath + "\\" + curNode.FilePath()); //список путей к файлам
				if (files != null) //заполнение узла данными по списку файлов
				{
					pState.files += AddChilds(curNode, files);
				}

				var dirs = _dirSource.GetDirectories(rootPath + "\\" + curNode.FilePath()); //список путей к директориям
				if (dirs != null)
				{
					pState.dirs += AddChilds(curNode, dirs);
					
					//foreach (var dn in dirNodes)
					foreach (var dn in curNode.Children)
					{
						nodes.Push(dn as EnumerableDFNode);
					}
				}
				_processChanged?.Invoke(pState);
			}
			return result;
		}		
	}

	public class ScanProcessSate
	{
		public string name { get; }
		public int dirs { get; set; } = 0;
		public int files { get; set; } = 0;

		public ScanProcessSate(string name)
		{
			this.name = name ?? throw new ArgumentNullException(nameof(name));
		}
	}
}