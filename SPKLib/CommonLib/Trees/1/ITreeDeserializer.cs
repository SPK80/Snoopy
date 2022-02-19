namespace CommonLib.Trees
{
    public interface INodesParser<T>
    {
        NodesDictionary<T> ParseNode(string txt);
        T ParseBranch(string txt);
        bool IsNode(string txt);
        string SerializeNode(NodesDictionary<T> root);
        string SerializeBranch(NodesDictionary<T> root);

    }

    

}
