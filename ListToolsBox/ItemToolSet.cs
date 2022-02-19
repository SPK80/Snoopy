using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SPKLib.TimeSpanExtensions;

namespace ListToolsBox
{
	//public enum ControllTags { Selector, btnMultySelect, btnShowTools, btnOpen, btnClose,
 //       btnRescan, btnScanNew, btnCancelScan, Progress, btnMoveUp, btnMoveDown }
 	
	internal class ItemToolSet : AbstractToolSet
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
            selector.CheckedChanged += (s, e) => OnSelect?.Invoke(this, e);
            selector.DoubleClick += (sender, e) => selectorEditor.Visible = true;
            Items.Add(selector);

            selectorEditor = new ToolStripTextBox();
            selectorEditor.Visible = false;
            selectorEditor.BackColor = Color.Yellow;
            selectorEditor.Leave += (s, e) => selectorEditor.Visible = false;
            selectorEditor.KeyUp += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    selector.Text = selectorEditor.Text;
                    selectorEditor.Visible = false;
                    OnRename?.Invoke(this, e);
                }
            };
            Items.Add(selectorEditor);
            //var cancelProgressImage = ImageList.Images["CancelProgress"];
            cancelProgress = new ToolStripButton
            {
                Height = Parent.ItemHeight - 2,
                //Size = new Size(bh, bh),
                Visible = true,
                Tag = tag,                
                CheckOnClick = false,
                Dock = DockStyle.Right,
                Overflow = ToolStripItemOverflow.AsNeeded,
            };
            cancelProgress.Click += (s, e) => OnCancelProgress?.Invoke(this, e);
            Items.Add(cancelProgress);
        }

        public event EventHandler OnCancelProgress;
        public event EventHandler OnSelect;
        public event EventHandler OnRename;

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
            set => selector.Value=value;
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
