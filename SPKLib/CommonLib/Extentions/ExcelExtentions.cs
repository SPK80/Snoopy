using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using MExcel = Microsoft.Office.Interop.Excel;
using WinApi = CommonLib.WindowsApi;

namespace CommonLib.Extentions.Excel
{
    public class ExcelWrap
    {
        public ExcelWrap(string activeObjectName = "", bool tryNewInstance = false)
        {
            if (activeObjectName != "")
                this.activeObjectName = activeObjectName;
            initApp(tryNewInstance);
        }

        public IEnumerable<MExcel.Workbook> OpenedWorkbooks()
        {
           return excelApp.Workbooks.Cast<MExcel.Workbook>();
        }

        private MExcel.Application excelApp;
        private string activeObjectName = "Excel.Application";
        
        private void initApp(bool tryNewInstance)
        {
            excelApp = null;
            if (!(tryNewInstance))
            {
                try
                {
                    excelApp = System.Runtime.InteropServices.Marshal.GetActiveObject(activeObjectName) as MExcel.Application;
                }
                catch { excelApp = null; };
            }
            if (excelApp == null) excelApp = new MExcel.Application();
        }

        private MExcel.Workbook getWorkbook(string fileName)
        {
            //ищем среди открытых книг
            MExcel.Workbook workbook = null;
            try
            {
                workbook = excelApp.Workbooks.Cast<MExcel.Workbook>().First(wb => wb.FullName == fileName);
            }
            catch (Exception) { workbook = null; }

            if (workbook != null) return workbook;
            //если нет среди открытых книг то открываем
            return excelApp.Workbooks.Open(
                fileName, // FileName
                false, // UpdateLinks
                false, //  ReadOnly
                Type.Missing, // Format
                Type.Missing, // Password
                Type.Missing, // WriteResPassword
                Type.Missing, // IgnoreReadOnlyRecommended
                Type.Missing, // Origin
                Type.Missing, // Delimiter
                true, // Editable
                Type.Missing, //  Notify
                Type.Missing, // Converter
                false, // AddToMru
                Type.Missing, // Local
                Type.Missing // CorruptLoad
            );
        }

        public MExcel.Range FindFirstInBook(string bookName, string what)
        {
            var awb = getWorkbook(bookName);
            return awb.FindFirst(what);
        }

        public IEnumerable<MExcel.Range> FindAllInBook(string bookName, string what)
        {
            var awb = getWorkbook(bookName);
            return awb.FindAll(what);
        }

        public void ShowFinded(MExcel.Range finded)
        {
            finded.Show_();
        }
        public void CloseWorkbook(string bookName)
        {
            var awb = getWorkbook(bookName);
            awb.Close();
        }


    }

    public static class ExcelExtentions
	{       
        public static void Show_(this MExcel.Range range)
		{
            
            if (range.Worksheet.Parent is MExcel.Workbook wb)
            {
                if (wb.Windows.Count > 0)
                {
                    var wnd = wb.Windows.Item[1];
                    wnd.Activate();
                    wb.Application.Visible = true;
                    ShowActivWindow(WinApi.Window.ShowCmd.SHOWMAXIMIZED);
                    range.Worksheet.Activate();
                    range.Activate();
                }            
            }            
        }


        //public static IntPtr ActivateWindowHandle()
        //{
        //	var pro = Process.GetProcessesByName("EXCEL");
        //	try
        //	{
        //		return pro.First().MainWindowHandle;
        //	}
        //	catch
        //	{
        //		return IntPtr.Zero;
        //	}
        //}

        public static void ShowActivWindow(WinApi.Window.ShowCmd nCmdShow)
		{
			var exPros = Process.GetProcessesByName("EXCEL");
			if (exPros.Count() < 1) return;
			var exPro = exPros.First();
			if (exPro != null)
			{
				bool result = WinApi.Window.ShowWindowAsync(exPro.MainWindowHandle, (int)nCmdShow);
			}
		}

        public static MExcel.Range FindFirst(this MExcel.Workbook book, object what)
        {            
            foreach (MExcel.Worksheet sh in book.Sheets)
            {
                var cs = sh.Cells;
                var fcs = cs.Find(what);
                if (fcs != null)
                    return fcs;
            }
            return null;
        }

        public static IEnumerable<MExcel.Range> FindAll(this MExcel.Workbook book, object what)
        {
            if (what != null)
            {
                foreach (MExcel.Worksheet sh in book.Sheets)
                {
                    var cs = sh.Cells;
                    var firstCell = cs.Find(what, Missing.Value, MExcel.XlFindLookIn.xlValues);
                    if (firstCell != null)
                    {
                        var firstRC = firstCell.Address;
                        var nextC = firstCell;
                        do
                        {
                            //nextC.Show();
                            yield return nextC;
                            nextC = cs.FindNext(nextC);

                        } while (nextC.Address != firstRC);
                    }
                }
            }
        }
    }

}
