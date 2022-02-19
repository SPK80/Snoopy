using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IndFin.Core.DFN
{
	[JsonObject]
	public class EnumerableDFNode : FileNode, IEnumerable<FileNode>, IIndexItem
	{
		/// <summary>
		///Конструктор по умолчанию (нужен для десериализатора)
		/// </summary>
		public EnumerableDFNode() : base() {}
		public EnumerableDFNode(DFData data, bool asFile) : base(data, asFile) {}


		public EnumerableDFNode AddChild(DFData data, bool asFile)
		{
			//if (Children == null)
			//	Children = new DFNodeCollection(this);
			var сhild = new EnumerableDFNode(data, asFile);
			Children.Add(сhild);
			return сhild;
		}

		public IEnumerator<FileNode> GetEnumerator()
		{
			//Инициализация стека FILO
			var nodes = new Stack<FileNode>(20);//начальный размер нада подобрать
			var rootNode = FindRoot();
			nodes.Push(rootNode);
			//Обработка начальной папки
			yield return rootNode;
			while (nodes.Count > 0)
			{
				//получаем текущую папки из стека
				var curNode = nodes.Pop();
				if (curNode.Children != null)
					foreach (var node in curNode.Children)
					{
						//if (node.Children.Count > 0)
						if (node.IsDir())
							nodes.Push(node);
						yield return node;
					}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}