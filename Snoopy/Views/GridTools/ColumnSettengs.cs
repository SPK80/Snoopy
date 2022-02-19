using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.Views.GridTools
{
    public struct ColumnSettengs
    {
        public string Name;
        public string Caption;
        public bool Visible;
        public int Index;
        public EventHandler OnClick;
    }

    public class ColumnsSettengs : List<ToolStripMenuItem>
    {
        public ColumnsSettengs(ColumnSettengs[] columnsSettengs=null)
        {
            if (columnsSettengs == null) return;
            foreach (var cs in columnsSettengs)
            {
                Add(NewColumnsMenuItem(cs));
            }
        }

        private ToolStripMenuItem NewColumnsMenuItem(ColumnSettengs columnSettengs)
        {
            var result = new ToolStripMenuItem();
            result.CheckOnClick = true;
            result.Name = columnSettengs.Name;
            result.Text = columnSettengs.Caption;
            result.Tag = columnSettengs.Index;
            result.Checked = columnSettengs.Visible;
            if (columnSettengs.OnClick != null)
                result.Click += columnSettengs.OnClick;
            return result;
        }
    }
}
