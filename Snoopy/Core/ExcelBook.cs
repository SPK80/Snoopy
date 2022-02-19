using CommonLib.Extentions.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snoopy.Core
{
    public class ExcelBook : Source
    {        
        public override string Name { get; protected set; }
        public override string Path { get; protected set; }
        public override string Extantion { get; protected set; }

        
        //public override bool Loaded => true;//throw new NotImplementedException();

        public override void Find(string what, params object[] options)
        {
            var ew = new ExcelWrap();
            //перебор всех выбранных источников (книг) в списке lbSources
            if (what == "") return;
            var finded = ew.FindAllInBook(Path, what);
            var c = finded?.Count() ?? 0;
            List<ExcelRangeResult> resultList = null;
            if (c > 0)
            {
                resultList = new List<ExcelRangeResult>();
                foreach (var r in finded)
                {
                    resultList.Add(new ExcelRangeResult(r));
                }
            }
            GotResults?.Invoke(this, resultList);
            //return resultList;
        }

        public ExcelBook(string name, string path) : base(name, path)
        { }

        public override void CancelProcess()
        {
            throw new NotImplementedException();
        }
       

        //private void addOpenedSourses(SourcesList resultList)
        //{
        //    var excelWrap = new ExcelWrap();
        //    var workbooks = excelWrap.OpenedWorkbooks();
        //    if (workbooks != null)
        //    {
        //        foreach (var wb in workbooks)
        //        {
        //            if (!resultList.Contains(wb.FullName))
        //                resultList.Add(new ExcelBook(wb.Name, wb.FullName));
        //        }
        //    }
        //}
        /// <summary>
        /// Получает Sources из памяти и из Favorites
        /// </summary>
        //private void view_OnTakeSources(IList resultList)
        //{
        //var excelWrap = new ExcelWrap();
        //var workbooks = excelWrap.OpenedWorkbooks();
        //if (workbooks == null) return;

        ////удаляем все не закреплённые(!Favorite) Sources
        //var lfs = resultList.Cast<LinkedFile>();
        //var ffav = lfs.FirstOrDefault(lf => !lf.Favorite);
        //while (ffav!=null)
        //{
        //    resultList.Remove(ffav);
        //    ffav = lfs.FirstOrDefault(lf => !lf.Favorite);
        //}
        ////получаем Sources из памяти
        //try
        //{
        //    foreach (var wb in workbooks)
        //    {
        //        view_AddSource(wb.FullName, wb.Name, false, resultList);
        //        //if (resultList.Cast<LinkedFile>().FirstOrDefault(lf=>lf.Path==wb.FullName)==null) //фильтр по уже загруженным
        //        //    resultList.Add(new LinkedFile(wb.FullName, false, FileTypes.Excel, wb.Name));
        //    }
        //}
        //catch (Exception ex)
        //{
        //    view.ShowMessage("Не пому получить список открытых книг. Excel чем то заблокирован.", null);
        //    LogHelper.Log.Write(ex, "view_OnTakeSources"+ workbooks);
        //}
        //}


    }
}
