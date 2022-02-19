using System.Collections.Generic;
using System.Data;
using System.Linq;
using SPKCollections.Extentions;

namespace SPKCollections
{

    public static class TableSerializer
    {
        private static string[] toRow(IEnumerable<string[]> table)
        {
            IEnumerable<string[]> results = null;
            foreach (var row in table)
            {
                if (results == null)
                    results = new[] { row };
                else
                    results = new MergingDecorator<string>(results, new[] { row }, "");
            }
            return results.First();
        }

        private static string rowToString(string[] row, string spliter)
        {
            return row.Aggregate((ac, s) => ac + spliter + s);
        }

        public static IEnumerable<string[]> ToRow(IEnumerable<string[]> table)
        {
            var td = new TableDecorator<string>(table);

            var header = td.Header;
            var count = td.Count();

            var headers = new List<string>();
            for (int i = 0; i < count; i++)
            {
                foreach (var h in header)
                {
                    headers.Add($"{h}{i}");
                }
            }

            var mergedResults = toRow(td);

            return new[] { headers.ToArray(), mergedResults };
        }

        public static IEnumerable<string[]> ToString(IEnumerable<string[]> table, string spliter)
        {
            foreach (var row in table)
            {
                var str = new[] { rowToString(row, spliter) };
                yield return str;

            }
        }      
    }


    //public interface IMerging<H, T>
    //{
    //    TablesMerger<H, T> GetMerged(T emptyField, ITable<H, T> table);
    //    TablesMerger<H, T> GetMerged(T emptyField, params ITable<H, T>[] tables);
    //}

    //public class TablesMerger<H, T> : DTODecorator<H, T>, IMerging<H, T>
    //{        
    //    private class TED
    //    {
    //        public ITable<H, T> Table;
    //        public IEnumerator<T[]> Enum;
    //        public bool Done;
    //    }

        
    //    private static T[] getEmptyRow(T emptyField, int wi)
    //    {
    //        var row = new T[wi];
    //        row.Fill(emptyField);
    //        return row;
    //    }      

    //    private H[] mergeHeader(ITable<H, T> table)
    //    {
    //        var hs = new List<H>();
    //        hs.AddRange(this.Header);
    //        hs.AddRange(table.Header);
    //        return hs.ToArray();
    //    }

    //    private static H[] mergeHeader(ITable<H, T>[] tables)
    //    {
    //        var hs = new List<H>();
    //        foreach (var table in tables)
    //        {
    //            hs.AddRange(table.Header);
    //        }
    //        return hs.ToArray();
    //    }

    //    private IEnumerable<T[]> mergeData(T emptyField, ITable<H, T>[] tables)
    //    {
    //        var tableAndDataEnumerators = tables.Select(t =>
    //        new TED { Table = t, Enum = t.Rows.GetEnumerator(), Done = false }).ToArray();

    //        while (true)
    //        {
    //            var agrDataRow = new List<T>();

    //            foreach (var ted in tableAndDataEnumerators)
    //            {
    //                T[] row;
    //                if (ted.Enum.MoveNext())
    //                {
    //                    row = ted.Enum.Current;
    //                }
    //                else
    //                {
    //                    row = getEmptyRow(emptyField, ted.Table.Header.Length);
    //                    ted.Done = true;
    //                }
    //                agrDataRow.AddRange(row);
    //            }
    //            if (tableAndDataEnumerators.Any(ted => !ted.Done))
    //                yield return agrDataRow.ToArray();
    //            else yield break;
    //        }
    //    }
        
    //    private IEnumerable<T[]> mergeData(T emptyField, ITable<H, T> table)
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
    //                var row = done0 ? enum0.Current : getEmptyRow(emptyField, Header.Length);
    //                mrgDataRow.AddRange(row);
    //                row = done1 ? enum1.Current : getEmptyRow(emptyField, table.Header.Length);
    //                mrgDataRow.AddRange(row);
    //                yield return mrgDataRow.ToArray();
    //            }
    //            else yield break;
    //        }
    //    }


    //    public TablesMerger(H[] header, IEnumerable<T[]> data) : base(header, data)
    //    {
    //    }

    //    public TablesMerger(ITable<H, T> table) : base(table.Header, table.Rows)
    //    {
    //    }

    //    public TablesMerger<H, T> GetMerged(T emptyField, ITable<H, T> table)
    //    {
    //        return new TablesMerger<H, T>(mergeHeader(table), mergeData(emptyField, table));
    //    }

    //    public TablesMerger<H, T> GetMerged(T emptyField, params ITable<H, T>[] tables)
    //    {
    //        var list = new List<ITable<H, T>>();
    //        list.Add(this);
    //        list.AddRange(tables);            
    //        return new TablesMerger<H, T>(mergeHeader(list.ToArray()), mergeData(emptyField, list.ToArray()));
    //    }
    //}

}
