using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionExtentions
{
    public static class Extentions
    {
        public static bool IsVoid<T>(this ICollection<T> self)
        {
            return self == null || self.Count() < 1;
        }

        //public static bool TryMoveNext(this IEnumerator ie)
        //{
        //    //bool hasCatch = false;
        //    int catchCount = 10000;
        //    while (catchCount>0)
        //    {
        //        try
        //        {
        //            return ie.MoveNext();
        //        }
        //        catch //(Exception ex)
        //        {
        //            //if (ex is )
        //            //    hasCatch = true;
        //        }
        //    }
        //    return false;
        //}
    }
}
