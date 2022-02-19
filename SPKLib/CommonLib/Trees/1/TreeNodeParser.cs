using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLib.Trees
{
    public class TreeNodeParser<T>
    {
        private INodesParser<T> parser;

        private IEnumerable<ATreeBranch<T>> getChildren(string[] values)
        {
            foreach (var value in values)
            {
                if (parser.IsNode(value))
                {
                    var node = parser.ParseNode(value).First();
                    var tn = new ATreeNode<T>(node.Key, getChildren(node.Value.ToStrings()));
                    yield return tn;
                }
                else
                {
                    var branch = parser.ParseBranch(value);
                    var tb = new ATreeBranch<T>(branch);
                    yield return tb;
                }
            }
        }

        public TreeNodeParser(INodesParser<T> parser)
        {
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public ATreeNode<T> Parse(string txt)
        {
            var root = parser.ParseNode(txt).First();
            return new ATreeNode<T>(root.Key, getChildren(root.Value.ToStrings()));
        }

        public string Serialize(ATreeNode<T> root)
        {
            var nd = new NodesDictionary<T>();
            nd.Add(root.Data, root.Children);

            return parser.SerializeNode(nd);
        }
         

    }

}
