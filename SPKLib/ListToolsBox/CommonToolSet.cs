using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListToolsBox
{
    //internal enum CommonButtonsState { allOn, allOff, selectedOne, selectedMany, };

    internal class CommonToolSet : AbstractToolSet
	{
        private ToolStripButton multySelectCheck;
        private ToolStripButton btnOpen;
        private ToolStripButton btnNew;
        private ToolStripButton btnClose;
        private ToolStripButton btnUpdate;
        private ToolStripButton btnCancel;
        private ToolStripButton btnMoveUp;
        private ToolStripButton btnMoveDown;

        //public void WatchItemToolSet(ItemToolSet itemToolSet)
        //{
        //    itemToolSet.OnSelect += (s, e) =>
        //    {
        //        true.SetEnabled(btnUpdate, btnClose);
        //    };
        //}

        public void SetEnabled(bool value, params string[] controlNames)
        {
            foreach (var cName in controlNames)
            {                
                foreach (var c in Items.Find(cName, true))
                {
                    c.Enabled = value;
                }                
            }
        }

        public IDictionary<string, bool> Enabled;
        
        public CommonToolSet(string name, ListToolsBox parent) : base(name, parent)
        {
            multySelectCheck = newButton("MultySelect", "Несколько");
            multySelectCheck.CheckOnClick = true;
            multySelectCheck.Checked = false;
            multySelectCheck.CheckedChanged += (s, e) => 
                OnMultySelectCheck?.Invoke(this, new CheckEventArgs(multySelectCheck.Checked));
            Items.Add(multySelectCheck);

            btnOpen = newButton("Open", "Открыть");             
            btnOpen.Click += (s, e) => OnOpen?.Invoke(this, e);
            Items.Add(btnOpen);            

            btnClose = newButton("Close", "Закрыть");            
            btnClose.Click += (s, e) => OnClose?.Invoke(this, e);
            Items.Add(btnClose);

            btnNew = newButton("New", "Новый");
            btnNew.Click += (s, e) => OnNew?.Invoke(this, e);
            Items.Add(btnNew);

            btnUpdate = newButton("Update", "Обновить");
            btnUpdate.Click += (s, e) => OnUpdate?.Invoke(this, e);
            Items.Add(btnUpdate);

            btnCancel = newButton("Cancel", "Отмена");
            btnCancel.Click += (s, e) => OnCancel?.Invoke(this, e);
            Items.Add(btnCancel);

            btnMoveUp = newButton("MoveUp", "Вверх");
            btnMoveUp.Click += (s, e) => OnMoveUp?.Invoke(this, e);
            Items.Add(btnMoveUp);

            btnMoveDown = newButton("MoveDown", "Вниз");
            btnMoveDown.Click += (s, e) => OnMoveDown?.Invoke(this, e);
            Items.Add(btnMoveDown);
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
