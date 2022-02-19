using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CommonLib.Trees
{
    public class ATreeNode<T>: ATreeBranch<T>
    {
        public ATreeNode(T data, IEnumerable<ATreeBranch<T>> children) : base(data)
        {
            Children = children?.ToArray();
            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        //public TreeChildrenCollection Children { get; }
        //private ATreeBranch<T>[] children;
        public ATreeBranch<T>[] Children
        {
            get;
            //protected set
            //{
            //    children = value;
            //    foreach (var child in children)
            //    {
            //        child.Parent = this;
            //    }
            //}
        }
        

        //public class TreeChildrenCollection : Collection<ATreeBranch<T>>
        //{
        //    protected ATreeNode<T> owner = null;

        //    public TreeChildrenCollection() { }

        //    public TreeChildrenCollection(ATreeNode<T> owner)
        //    {
        //        this.owner = owner;
        //    }

        //    /// <summary>
        //    /// Вставка с проверкой уникальности item
        //    /// </summary>
        //    protected override void InsertItem(int index, ATreeBranch<T> item)
        //    {
        //        if (!Contains(item))
        //        {
        //            base.InsertItem(index, item);
        //            item.parent = owner;
        //        }
        //    }

        //    /// <summary>
        //    /// Удаление с разрывам связи с Parent
        //    /// </summary>
        //    protected override void RemoveItem(int index)
        //    {
        //        this[index].parent = null;
        //        base.RemoveItem(index);
        //    }

        //    public new IEnumerator<ATreeBranch<T>> GetEnumerator()
        //    {
        //        if (Count == 0 && owner != null)
        //        {
        //            var path = owner.GetPath();
        //            foreach (var ch in owner.getChildren(path))
        //            {
        //                Add(ch);
        //            }
        //        }
        //        return base.GetEnumerator();
        //    }
        //}

    }



}
