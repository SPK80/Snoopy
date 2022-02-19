using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CommonLib.Extentions
{
    public static class ClipboardExt
    {
        public static string[][] TableFromClipboard()
        {
            IDataObject dataInClipboard = Clipboard.GetDataObject();
            string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);
            if (stringInClipboard == null) return null;

            string[] rowsInClipboard = stringInClipboard.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var results = new List<string[]>();
            foreach (var row in rowsInClipboard)
            {
                results.Add(row.Split('\t'));
            }
            return results.ToArray();
        }

    }
}
