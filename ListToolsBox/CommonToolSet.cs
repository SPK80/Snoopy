using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListToolsBox
{
    internal class CommonToolSet : AbstractToolSet
	{
        private ToolStripButton multySelectChack;
        private ToolStripButton btnOpen;
        private ToolStripButton btnNew;
        private ToolStripButton btnClose;
        private ToolStripButton btnUpdate;
        private ToolStripButton btnCancel;
        private ToolStripButton btnMoveUp;
        private ToolStripButton btnMoveDown;

        public CommonToolSet(string name, ListToolsBox parent) : base(name, parent)
        {
            multySelectChack = newButton("multySelect", "Несколько");
            multySelectChack.CheckOnClick = true;
            multySelectChack.Checked = false;
            multySelectChack.CheckedChanged += (s, e) => 
                OnMultySelectCheck?.Invoke(this, new CheckEventArgs(multySelectChack.Checked));

            btnOpen = newButton("Open", "Открыть");           
            btnOpen.Click += (s, e) => OnOpen?.Invoke(this, e);
            
            btnClose = newButton("Close", "Закрыть");            
            btnClose.Click += (s, e) => OnClose?.Invoke(this, e);

            btnNew = newButton("New", "Новый");
            btnNew.Click += (s, e) => OnNew?.Invoke(this, e);

            btnUpdate = newButton("Update", "Обновить");
            btnUpdate.Click += (s, e) => OnUpdate?.Invoke(this, e);

            btnCancel = newButton("Cancel", "Отмена");
            btnCancel.Click += (s, e) => OnCancel?.Invoke(this, e);

            btnMoveUp = newButton("MoveUp", "Вверх");
            btnMoveUp.Click += (s, e) => OnMoveUp?.Invoke(this, e);

            btnMoveDown = newButton("MoveDown", "Вниз");
            btnMoveDown.Click += (s, e) => OnMoveDown?.Invoke(this, e);            
            
        }

        public event EventHandler<CheckEventArgs> OnMultySelectCheck;
        public event EventHandler OnOpen;
        public event EventHandler OnClose;
        public event EventHandler OnNew;
        public event EventHandler OnUpdate;
        public event EventHandler OnCancel;
        public event EventHandler OnMoveUp;
        public event EventHandler OnMoveDown;
        
        private ToolStripButton newButton(string name, string text)
        {
            return new ToolStripButton
            {
                Height = Parent.ItemHeight - 2,
                Name = name,
                Text = text,
                Dock = DockStyle.Left,
                Overflow = ToolStripItemOverflow.AsNeeded,
            };
        }
    }

    public class CheckEventArgs : EventArgs
    {
        public bool Checked;

        public CheckEventArgs(bool @checked)
        {
            Checked = @checked;
        }
    }
}
