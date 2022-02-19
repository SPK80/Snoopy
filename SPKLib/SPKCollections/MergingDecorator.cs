using System;
using System.Collections;
using System.Collections.Generic;
using SPKCollections.Extentions;

namespace SPKCollections
{
    //public class ConcatDecorator<T>: IEnumerable<T[]>
    //{
    //    private IEnumerable<T[]> _enum;
    //    private T[] row;

    //    public ConcatDecorator(IEnumerable<T[]> _enum, )
    //    {
    //    }
    //}

    /// <summary>
    /// Объединяет таблицы по столбцам
    /// </summary>
    /// <typeparam name="T">тип ячеек таблиц</typeparam>
    public class MergingDecorator<T>: IEnumerable<T[]>
    {
        private IEnumerable<T[]> items1, items2;
        private T emptyField;

        public MergingDecorator(IEnumerable<T[]> items1, IEnumerable<T[]> items2, T emptyField)
        {
            this.items1 = items1 ?? throw new ArgumentNullException(nameof(items1));
            this.items2 = items2 ?? throw new ArgumentNullException(nameof(items2));
            this.emptyField = emptyField;
        }

        public IEnumerator<T[]> GetEnumerator()
        {
            var enum1 = items1.GetEnumerator();
            var enum2 = items2.GetEnumerator();
            int len1=0, len2 = 0;
            bool init = false;
            while (true)
            {
                var mrgDataRow = new List<T>();

                var done1 = enum1.MoveNext();
                var done2 = enum2.MoveNext();
                if (!init)
                {
                    len1 = enum1.Current.Length;
                    len2 = enum2.Current.Length;
                    init = true;
                }                

                if (init && (done1 || done2))
                {
                    var row = done1 ? enum1.Current : emptyField.NewFillArray(len1);
                    mrgDataRow.AddRange(row);

                    row = done2 ? enum2.Current : emptyField.NewFillArray(len2);
                    mrgDataRow.AddRange(row);

                    yield return mrgDataRow.ToArray();
                }
                else yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
