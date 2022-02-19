using System;
using MExcel = Microsoft.Office.Interop.Excel;
using CommonLib.Extentions.Excel;
using CommonLib.LogHelper;

namespace Snoopy
{
    public abstract class ContentResult
    {
        public abstract string Text { get; }
        public abstract string Address { get; }
        public abstract string Page { get; }
        public abstract string SourceFile { get; }
    }

    public class ExcelRangeResult : ContentResult
    {
        private string tryGet(Func<string> getter)
        {
            string result = "";
            try
            {
                result = getter();
            }
            catch (Exception ex){ Log.Write(ex, $"{this.ToString()}.tryGet({getter.ToString()})"); }
            return result;
        }

        private MExcel.Range excelRange;

        public ExcelRangeResult(MExcel.Range excelRange)
        {
            this.excelRange = excelRange ?? throw new ArgumentNullException(nameof(excelRange));
        }
        public void Show() => excelRange.Show_();

        public override string Text => tryGet(()=> excelRange?.Text ?? "");

        public override string Address => tryGet(() => excelRange?.Address ?? "");

        public override string SourceFile => tryGet(() => excelRange?.Parent?.Parent?.Name ?? "");

        public override string Page => tryGet(() => excelRange?.Parent?.Name ?? "");
    }

}
