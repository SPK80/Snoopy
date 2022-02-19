using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonLib.Trees
{
    public static class EnumerableExtensions
    {
        public static string[] ToStrings(this IEnumerable collection, int count=0)
        {
            if (count==0)
            {                 
                foreach (var item in collection)
                    count++;
            }

            var results = new string[count];
            int i = 0;
            foreach (var item in collection)
                results[i++] = item.ToString();
            return results;
        }

        
    }

    public class EnumCustomNode<T> : CustomNode<T>, IEnumerable<CustomNode<T>>
	{	
		public EnumCustomNode() : base() { }
		public EnumCustomNode(T data) : base(data) { }

		public IEnumerator<CustomNode<T>> GetEnumerator()
		{
			//Инициализация стека FILO
			var nodes = new Stack<CustomNode<T>>(20);//начальный размер нада подобрать			
			var rootNode = RootNode();
			nodes.Push(rootNode);
			//Обработка начальной папки                
			yield return rootNode;
			while (nodes.Count > 0)
			{
				//получаем текущую папки из стека
				var curNode = nodes.Pop();

				foreach (var node in curNode.Children)
				{
					if (node.Children.Count > 0)
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
