using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SPKCollections
{
    //public class DTODecorator<H, T> : ITable<H, T>, IEnumerable<Dictionary<H, T>>
    //{
    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();        

    //    public H[] Header { get; }
    //    public IEnumerable<T[]> Rows { get; }

    //    public DTODecorator(ITable<H, T> table)
    //    {
    //        Header = table.Header;
    //        Rows = table.Rows;
    //    }
    //    public DTODecorator(H[] header, IEnumerable<T[]> rows)
    //    {
    //        Header = header ?? throw new ArgumentNullException(nameof(header));
    //        Rows = rows ?? throw new ArgumentNullException(nameof(rows));
    //    }

    //    public IEnumerator<Dictionary<H, T>> GetEnumerator()
    //    {
    //        foreach (var row in Rows)
    //        {
    //            var d = new Dictionary<H, T>();
    //            for (int i = 0; i < Math.Min(Header.Length, row.Count()); i++)
    //            {
    //                d.Add(Header[i], row[i]);
    //            }
    //            yield return d;
    //        }
    //    }         
    //}
}
