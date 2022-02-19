namespace CommonLib.Trees
{
    public class ATreeBranch<T>
    {
        public ATreeNode<T> Parent { get; protected internal set; }
        public T Data { get; }
        public string GetPath(string delimiter)
        {
            return (Parent != null) ? Parent.GetPath(delimiter) + delimiter + Data.ToString() : Data.ToString();
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public ATreeBranch(T data)
        {
            Parent = null;
            Data = data;
        }
    }
}
