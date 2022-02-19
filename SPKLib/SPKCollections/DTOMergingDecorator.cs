using System.Collections.Generic;
using SPKCollections.Extentions;

namespace SPKCollections
{
    //public class DTOMergingDecorator<H, T> : DTODecorator<H, T>, IMergingTable<H, T>
    //{
    //    private IEnumerable<T[]> mergeRows(ITable<H, T> table, T emptyField)
    //    {
    //        var enum0 = Rows.GetEnumerator();
    //        var enum1 = table.Rows.GetEnumerator();
    //        while (true)
    //        {
    //            var mrgDataRow = new List<T>();

    //            var done0 = enum0.MoveNext();
    //            var done1 = enum1.MoveNext();
    //            if (done0 || done1)
    //            {
    //                var row = done0 ? enum0.Current : emptyField.NewFillArray(this.Header.Length);
    //                mrgDataRow.AddRange(row);
    //                row = done1 ? enum1.Current : emptyField.NewFillArray(table.Header.Length);
    //                mrgDataRow.AddRange(row);
    //                yield return mrgDataRow.ToArray();
    //            }
    //            else yield break;
    //        }
    //    }

    //    public DTOMergingDecorator(ITable<H, T> table) : base(table)
    //    {
    //    }

    //    public DTOMergingDecorator(H[] header, IEnumerable<T[]> rows) : base(header, rows)
    //    {
    //    }
               
    //    public IMergingTable<H, T> GetMerged(ITable<H, T> table, T emptyField)
    //    {
    //        var hs = new List<H>();
    //        hs.AddRange(Header);
    //        hs.AddRange(table.Header);
    //        var mergedHeader = hs.ToArray();

    //        return new DTOMergingDecorator<H, T>(mergedHeader, mergeRows(table, emptyField));
    //    }
    //}
}
