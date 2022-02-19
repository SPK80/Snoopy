using System;
using System.Linq;
using CommonLib.Trees;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.JsonUtils
{
    public class JsonTreeConverter<T>
    {
        private JToken getJObject(ATreeBranch<T> branch)
        {
            if (branch is ATreeNode<T>)
            {
                var node = branch as ATreeNode<T>;
                var ja = node.Children.Select(n => getJObject(n)).ToArray();
                return new JObject{ dataConverter.ToString(node.Data) , new JArray(ja) };
            }
            else
            {
                return new JValue(branch.Data.ToString());
            }
        }       

        public string Serialize(ATreeBranch<T> branch)
        {            
            return getJObject(branch).ToString();            
        }

        public ATreeBranch<T> Deserialize(string txt)
        {
            var jt = JToken.Parse(txt);
            return getTreeBranch(jt);
        }

        private ATreeBranch<T> getTreeBranch(JToken token)
        {
            if (token is JObject)
            {
                var vs = (token as JObject).First as JProperty;
                //var jp = (token as JObject).Property;
                //var f = vs[0].First();
                var n = vs.Name;
                var v = vs.Value as JArray;
                return new ATreeNode<T>(dataConverter.FromString(n), v.Select(c=> getTreeBranch(c)));
            }
            else if (token is JToken)
            {
                return new ATreeBranch<T>(token.Value<T>());
            }
            else throw new System.Exception("Неверный тип токена");

        }
        private IDataConverter<T> dataConverter;

        public JsonTreeConverter(IDataConverter<T> dataConverter)
        {
            this.dataConverter = dataConverter ?? throw new ArgumentNullException(nameof(dataConverter));
        }
    }

    public interface IDataConverter<T>
    {
        string ToString(T data);
        T FromString(string str);
    }
}