using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ListToolsBox
{
	//public enum ControllTags { Selector, btnMultySelect, btnShowTools, btnOpen, btnClose,
 //       btnRescan, btnScanNew, btnCancelScan, Progress, btnMoveUp, btnMoveDown }
 	
	internal class ItemToolSet : AbstractToolSet, IToolSet
    {
        private ToolStripButtonProgress selector;
        private ToolStripTextBox selectorEditor;
        private ToolStripButton cancelProgress;

        #region public

        public ItemToolSet(object tag, ListToolsBox parent): base(tag.ToString(), parent)
        {
            Tag = tag;
            BackColor = Parent.BackColor;
            Font = Parent.Font;            
            selector = new ToolStripButtonProgress
            {
                Name = this.Name,
                Text = this.Name,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Width = Width - 20,
                Height = Parent.ItemHeight - 2,
                CheckOnClick = true,
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Visible = true,
                Overflow = ToolStripItemOverflow.Never,
            };
            selector.CheckedChanged += (s, e) =>
            {
                if (Selected)
                    OnSelect?.Invoke(this, e);
                else
                    OnDeSelect?.Invoke(this, e);
            };
            Items.Add(selector);

            selector.DoubleClick += (s, e) =>
            {
                selector.Visible = false;
                selectorEditor.Text = selector.Text;
            };
            
            selector.VisibleChanged += (s, e) => selectorEditor.Visible = !selector.Visible;
            
            selectorEditor = new ToolStripTextBox
            {
                Visible = false,
                BackColor = Color.Yellow,
                Overflow = ToolStripItemOverflow.Never,
            };
            
            selectorEditor.LostFocus += (s, e) => 
            selector.Visible = true;
            selectorEditor.KeyUp += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    selector.Text = selectorEditor.Text;
                    selector.Visible = true;
                    OnRename?.Invoke(this, e);
                }
            };
            Items.Add(selectorEditor);
            //var cancelProgressImage = ImageList.Images["CancelProgress"];
            cancelProgress = new ToolStripButton
            {
                Height = Parent.ItemHeight - 2,
                //Size = new Size(bh, bh),
                Visible = false,
                CheckOnClick = false,
                Dock = DockStyle.Right,
                Overflow = ToolStripItemOverflow.Never,
            };
            cancelProgress.Click += (s, e) => OnCancelProgress?.Invoke(this, e);
            Items.Add(cancelProgress);
        }

        public event EventHandler OnCancelProgress;
        public event EventHandler OnSelect;
        public event EventHandler OnDeSelect;
        public event EventHandler OnRename;
        public event EventHandler OnStartProgress;
        public event EventHandler OnStopProgress;


        public new string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                if (selector != null)
                    selector.Text = value;
            }
        }

        public bool Selected
		{
			get => selector.Checked;
			set => selector.Checked = value;
		}

		public int ProgressValue
		{
            get => selector.Value;
            set 
            {
                if (selector.Value<0 && value>=0)
                    OnStartProgress?.Invoke(this, EventArgs.Empty);
                if (selector.Value >= 0 && value < 0)
                { 
                    OnStopProgress?.Invoke(this, EventArgs.Empty);
                    selector.Text = Name;
                }
                selector.Value = value;
                cancelProgress.Visible = (value >= 0);                
            }
        }

        public string ProgressText
        {
            get => (ProgressValue >= 0) ? selector.Text : "";

            set
            {
                if (ProgressValue >= 0) selector.Text = value;
            }
        }

        public bool Progressing
        {
            get => cancelProgress.Visible;
            set => cancelProgress.Visible=value;
        }

        #endregion public

        /// Переопределение события 
        protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (selector!=null)
				selector.Width = Width - 12;
		}

		//private ToolStripButtonProgress NewIndexSelector(Size size)
		//{
		//	var result = new ToolStripButtonProgress
		//	{
		//		Name = this.Name,
		//		Text = this.Name,
		//		TextAlign = ContentAlignment.MiddleLeft,
		//		AutoSize = false,
		//		Width = Width - 20,
		//		Height = Parent.ItemHeight - 2,
		//		CheckOnClick = true,
		//		DisplayStyle = ToolStripItemDisplayStyle.Text,
		//		Visible = true,
		//		Tag = ControllTags.Selector
		//	};
  //          result.Overflow = ToolStripItemOverflow.Never;
  //          result.CheckedChanged += (s, e) => OnSelect(this, e);
  //          result.DoubleClick += (sender, e)=> selectorEditor.Visible = true;

		//	return result;			
		//}
		
		private int toolTipsCounter = 0;

		public void ShowMessage(string msg)
		{
			var tt = new ToolTip(Parent.Container);
			tt.Show(msg, this, 0, toolTipsCounter*18, 3000);
			toolTipsCounter++;
			if (toolTipsCounter > 2) toolTipsCounter = 0;
		}

	}
}
