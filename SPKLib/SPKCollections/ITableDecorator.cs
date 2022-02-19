using System.Collections.Generic;

namespace SPKCollections
{
    public interface ITable<T> : IEnumerable<T[]>
    {
        string[] Header { get; }
    }
}