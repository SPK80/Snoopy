using System.Text;
using System.Collections.ObjectModel;
using CommonLib.Extentions.String;

namespace CommonLib.Trees
{
    public class CustomNode<T>
    {
		public T Data { get; set; }

		public NodeCollection Children { get; }
				
		public CustomNode()
		{
			Children = new NodeCollection(this);
		}

		public CustomNode(T data) : this()
		{
			this.Data = data;
		}

        /// <summary>
        /// Добавляет созданный сhild в коллекцию
        /// </summary>
        /// <param name="сhild"></param>
        /// <returns>CustomNode сhild</returns>
        protected CustomNode<T> RegChild(CustomNode<T> сhild)
		{
			this.Children.Add(сhild);
			return сhild;
		}

        /// <summary>
        /// Создаёт новый сhild
        /// </summary>
        /// <param name="name"></param>
        /// <returns>CustomNode сhild</returns>
        protected CustomNode<T> AddChild(T data)
		{
			return RegChild(new CustomNode<T>(data));			
		}

		private CustomNode<T> _parent;

		protected CustomNode<T> Parent
		{
			set
			{
				//Если связь c Parent уже была, или нужно обнулить связь (value == null),
				//то сначала удаляем текущий Node из коллекции у старого Parent
				if (_parent != null || value == null)
				{
					_parent.Children.Remove(this);
				}
				else
				{
					value.Children.Add(this);
				}
				_parent = value;
			}
            get { return _parent; }
		}

		public override string ToString()
		{
            var rv = new StringBuilder();
            if (Data != null)
                rv.Append(Data.ToString());
            if (Children != null)
                foreach (CustomNode<T> ch in Children)
    			{
				    SubNodeToString(ch, rv);
			    }
			return rv.ToString();
		}

		protected int Level
		{
			get
			{
				return Parent != null ? Parent.Level + 1 : 0;
			}
		}

		private void SubNodeToString(CustomNode<T> n, StringBuilder sb)
		{
			sb.Append("\n" + "\t".Repeat(n.Level));
			sb.Append(n.Data.ToString());
			sb.Append(string.Format(" (Parent: {0})", n._parent.Data.ToString()));
			foreach (CustomNode<T> ch in n.Children)
			{
				SubNodeToString(ch, sb);
			}
		}

		public CustomNode<T> RootNode()
		{
			var n = this;
			while (n.Level > 0) 
				{ n = n.Parent; }
				
			return n;
		}

		/// <summary>
		/// Коллекция объектов CustomNode<T>
		/// </summary>
		public class NodeCollection : Collection<CustomNode<T>>
		{
            CustomNode<T> _owner;
			internal NodeCollection(CustomNode<T> owner)
			{
				_owner = owner;
			}

			/// <summary>
			/// Вставка с проверкой уникальности item
			/// </summary>			
			protected override void InsertItem(int index, CustomNode<T> item)
			{
				if (!this.Contains(item))
				{
					base.InsertItem(index, item);
					item._parent = _owner;
				}
			}

			/// <summary>
			/// Удаление с разрывам связи с Parent
			/// </summary>			
			protected override void RemoveItem(int index)
			{
				this[index]._parent = null;
				base.RemoveItem(index);
			}
		}
	}

}
